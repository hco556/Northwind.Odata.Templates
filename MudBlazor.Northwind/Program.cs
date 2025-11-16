using MetadataExtractor;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MudBlazor;
using MudBlazor.Extensions;
using MudBlazor.Northwind;
using MudBlazor.Northwind.Attributes;
using MudBlazor.Northwind.Components;
using MudBlazor.Northwind.Data;
using MudBlazor.Northwind.Services;
using MudBlazor.Northwind.T4;
using MudBlazor.Services;
using Northwind.Odata.Api.Client;
using Shared.Models;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Reflection;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
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
app.Use(MudExWebApp.MudExMiddleware);
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();


//Setting up MudBlazor.Extensions involves three steps:

//Update the _Imports.razor with the following lines:

//@using MudBlazor.Extensions
//@using MudBlazor.Extensions.Components
//@using MudBlazor.Extensions.Components.ObjectEdit
//Register MudBlazor.Extensions in your Startup.cs in the ConfigureServices method.

//// use this to add MudServices and the MudBlazor.Extensions
//builder.Services.AddMudServicesWithExtensions();

//// or this to add only the MudBlazor.Extensions but please ensure that this is added after mud servicdes are added. That means after `AddMudServices`
//builder.Services.AddMudExtensions();
//(Optional)Define default dialogOptions.

//builder.Services.AddMudServicesWithExtensions(c =>
//{
//    c.WithDefaultDialogOptions(ex =>
//    {
//        ex.Position = DialogPosition.BottomRight;
//    });
//});
//if your are running on Blazor Server side, you should also use the MudBlazorExtensionMiddleware you can do this in your startup or program.cs by adding the following line on your WebApplication:

//    app.Use(MudExWebApp.MudExMiddleware);
//(Optional) if you have problems with automatic loaded styles you can also load the styles manually by adding the following line to your index.html or _Host.cshtml

//<link id="mudex-styles" href="_content/MudBlazor.Extensions/mudBlazorExtensions.min.css" rel="stylesheet">
//If you have loaded styles manually you should disable the automatic loading of the styles in the AddMudExtensions or AddMudServicesWithExtensions method. You can do this by adding the following line to your Startup.cs in the ConfigureServices method.

//builder.Services.AddMudServicesWithExtensions(c => c.WithoutAutomaticCssLoading());
