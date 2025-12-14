using InteractiveMudBlazorServer_Net9.Components;
using InteractiveMudBlazorServer_Net9.Services;
using InteractiveMudBlazorServer_Net9Authorization.Policies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using MudBlazor;
using MudBlazor.Services;
using MudExtensions.Services;
using Serilog;
using Serilog.Events;
using System.Reflection;
using Serilog.Extensions.Hosting;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
var silenceLogs = args.Contains("--quiet-logs");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", silenceLogs ? LogEventLevel.Warning : LogEventLevel.Information)
    .WriteTo.Console()
    .WriteTo.File($"{WebAppConfig.LogsPath}/log-.log", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();
// Add MudBlazor services
builder.Services.AddMudServices();
builder.Services.AddMudExtensions();
//builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration)
//        .EnableTokenAcquisitionToCallDownstreamApi()
//        .AddInMemoryTokenCaches();

builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<IAuthorizationHandler, ClaimAuthorizationHandler>();
// Add Controllers

//builder.WebHost.UseStaticWebAssets();

//builder.Services.AddCascadingAuthenticationState();


builder.Services.AddHttpClient<IApiService, ApiService>(configureClient =>
{
    configureClient.BaseAddress = new Uri(builder.Configuration.GetSection("WebApi:Url").Value);
});
var dataDir = WebAppConfig.DataPath;
if (!Directory.Exists(dataDir))
{
    Directory.CreateDirectory(dataDir);
}

var dbDir = WebAppConfig.DatabasePath;
if (!Directory.Exists(dbDir))
{
    Directory.CreateDirectory(dbDir);
}

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlite($"Data Source = {Path.Combine(dbDir, "interactiveWebApp.db")}"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
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
static void SetupHttpClient(IServiceProvider provider, HttpClient client)
{
    var config = provider.GetRequiredService<WebAppConfig>();

    client.BaseAddress = config.BaseUrl;
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", config.APIKey);
    client.DefaultRequestHeaders.Add("requestcompressed", "0");
}