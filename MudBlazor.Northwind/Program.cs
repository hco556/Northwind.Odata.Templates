using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MudBlazor.Northwind;
using MudBlazor.Northwind.Components;
using MudBlazor.Northwind.Services;
using MudBlazor.Services;


using Northwind.Odata.Api.Client;
using Northwind.Odata.Api.Models;
using Shared.Models;
using Shared.Models.Attributes;
using Shared.Models.Data;
using Shared.Models.Shared.ViewModels;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
//builder.Services.AddTransient<RemoteApiService>();
//builder.Services.AddUserAccessTokenHttpClient("demoApiClient", configureClient: client =>
//{
//    client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
//});

builder.Services.AddHttpClient<ApiService>("apiclient",client =>
{
    client.BaseAddress = new Uri("https://localhost:5000");

});

var employee = new EmployeeViewModel();
var Properties = typeof(EmployeeViewModel).GetProperties()
        .Where(p => Attribute.IsDefined(p, typeof(DisplayNameAttribute)))
        .ToList();
var propertySets = new List<PropertySet>();
foreach (var property in Properties)
{
    PropertyInfo Property = property;
    object PropertyValue = Property?.GetValue(employee);
    string label = Property?.GetDisplayName();
    Type PropertyType = Property?.PropertyType;
    FormControlAttribute? formControlAttribute = Property.GetAttribute<FormControlAttribute>();
    var formControlType = FormControlType.Text;
    var isDisabled = false;
    var isRequired = false;
    if (formControlAttribute != null)
    {
        formControlType = formControlAttribute.Type;
        isRequired = formControlAttribute.IsRequired;
        isDisabled = formControlAttribute.IsDisabled;
    }

    var propertySet = new PropertySet(property.Name, PropertyType, formControlType, label, isRequired, isDisabled);

  
    propertySets.Add(propertySet);
}
;
var authProvider = new AnonymousAuthenticationProvider();

// Create request adapter using the HttpClient-based implementation
var adapter = new HttpClientRequestAdapter(authProvider);
var client = new NorthwindClient(adapter);
builder.Services.AddSingleton<NorthwindClient>(client);
KiotaRequestBuilder kiotaRequestBuilder = new KiotaRequestBuilder(client);
builder.Services.AddSingleton<KiotaRequestBuilder>(kiotaRequestBuilder);
//builder.Services.AddScoped<IOrderRepository, OrderRepository>();
//builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
