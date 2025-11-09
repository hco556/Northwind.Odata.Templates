using MudBlazor.Northwind.Components;
using MudBlazor.Northwind.Services;
using MudBlazor.Services;

using Northwind.api.Repositories;

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
    client.BaseAddress = new Uri("https://localhost:7021");

});
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
