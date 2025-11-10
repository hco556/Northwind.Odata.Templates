using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Northwind.Server.Persistence;
using Northwind.Shared.Employees.Validators;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddDbContext<NorthwindDataContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindCS"));
   // options.UseInMemoryDatabase("NorthwindCS");
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseWebAssemblyDebugging();
}

app.MapScalarApiReference();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseBlazorFrameworkFiles();
app.MapFallbackToFile("index.html");

app.MapControllers();

app.Run();

