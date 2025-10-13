using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using Northwind.api.Data;
using Northwind.api.Extensions;
using Northwind.api.Repositories;
using Northwind.api.Repositories.v2;
using OData.API.Helpers;
using Shared.Models;
using Shared.Models.v1;
using Shared.Models.v2;
using Microsoft.EntityFrameworkCore.InMemory;
using Swashbuckle.AspNetCore.Community.OData.DependencyInjection;
using System.Text.Json.Serialization;
//using Shared.Models; // Adjust namespace to your model location
//dotnet add package Microsoft.Extensions.Logging.Console
//dotnet add package Microsoft.ApplicationInsights.AspNetCore
var builder = WebApplication.CreateBuilder(args);

// Add logging: Console and Application Insights
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
//builder.Logging.AddApplicationInsights(
//    configureTelemetryConfiguration: (config) =>
//        config.ConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"],
//    configureApplicationInsightsLoggerOptions: _ => { }
//);

// Add Application Insights telemetry
//builder.Services.AddApplicationInsightsTelemetry();
builder.Services.AddDbContext<NorthwindContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnection"));
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Add services to the container.
builder.Services.AddControllers()
   .AddOData(options =>
    {
        var builder = new ODataConventionModelBuilder();
        builder.EnumType<Shared.Models.TestModels.Enums.CustomerType>();
        builder.EntitySet<Shared.Models.TestModels.Customer>("Customers");
        builder.EntitySet<Product>("Products"); // Example entity
        builder.EntitySet<Category>("Categories"); // Example entity
        builder.EntitySet<Order>("Orders"); // Example entity
        builder.EntityType<Shared.Models.TestModels.WeatherForecast>().HasKey(f => f.Id);
        builder
            .EntityType<Shared.Models.TestModels.WeatherForecast>()
            .HasDeleteRestrictions()
            .IsDeletable(false)
            .HasDescription("Not supported");
        builder
            .EntityType<Shared.Models.TestModels.WeatherForecast>()
            .HasUpdateRestrictions()
            .IsUpdatable(false)
            .HasDescription("Not supported");
        builder
            .EntityType<Shared.Models.TestModels.WeatherForecast>()
            .HasInsertRestrictions()
            .IsInsertable(false)
            .HasDescription("Not supported");

        builder.EntitySet<Shared.Models.TestModels.WeatherForecast>("WeatherForecasts");
        builder.EntityType<Shared.Models.TestModels.Customer>().HasKey(f => f.Id);
        options.AddRouteComponents("odata", builder.GetEdmModel())
            .Select().Filter().OrderBy().Expand().Count().SetMaxTop(1000);
        // v1 EDM
        var v1Builder = new ODataConventionModelBuilder();
        v1Builder.EntitySet<Shared.Models.v1.Customer>("Customers");

        // Add other v1 entities as needed
        options.AddRouteComponents("odata/v1", v1Builder.GetEdmModel())
            .Select().Filter().OrderBy().Expand().Count().SetMaxTop(1000);

        // v2 EDM
        var v2Builder = new ODataConventionModelBuilder();
        v2Builder.EntitySet<Shared.Models.v2.Customer>("Customers");
        v2Builder.EntitySet<Product>("Products"); // Example entity
        // Add other v2 entities as needed
        options.AddRouteComponents("odata/v2", v2Builder.GetEdmModel())
            .Select().Filter().OrderBy().Expand().Count().SetMaxTop(1000);

        // You can add more versions as needed
        // Register OData entity sets here
    //    var odataBuilder = new ODataConventionModelBuilder();

    //    odataBuilder.EntitySet<Product>("Products"); // Example entity
    ////    odataBuilder.EntitySet<Category>("Categories"); // Example entity
    //    options.AddRouteComponents("odata", odataBuilder.GetEdmModel())
    //           .Select()
    //           .Filter()
    //           .OrderBy()
    //           .Expand()
    //           .Count()
    //           .SetMaxTop(1000);
    }).AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.MaxDepth = 100; // Increase max depth to handle complex objects
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;//ReferenceHandler.IgnoreCycles;
    });
//builder.Services.TryAddSingleton<IODataModelProvider, MyODataModelProvider>();

//builder.Services.TryAddEnumerable(

//builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<MatcherPolicy, MyODataRoutingMatcherPolicy>());
builder.Services.AddSwaggerGenOData(opt =>
    opt.SwaggerDoc(
        "v1",
        "odata",
        new OpenApiInfo { Title = "My Open API", Version = "v1" }
    )
);

// Add Swagger/OpenAPI support
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(
//    opt => opt.ResolveConflictingActions(a => a.First()));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository,OrderRepository>();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole();
});

//builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["ApplicationInsights:InstrumentationKey"]);
builder.Services.AddDbContext<ODataAPIDbContext>(options =>
    options.UseInMemoryDatabase("ODataAPIDb"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseSwagger();

app.MapControllers();
app.UseSwaggerUI(c => c.SwaggerEndpoint("v1/swagger.json", "My OData API"));

using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var db = serviceScope.ServiceProvider.GetRequiredService<ODataAPIDbContext>();

    ODataAPIDbHelper.SeedDb(db);
}
app.Run();
