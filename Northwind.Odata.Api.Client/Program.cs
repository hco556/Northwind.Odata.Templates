// See https://aka.ms/new-console-template for more information
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Northwind.Odata.Api.Client.Odata;
using Northwind.Odata.Api.Client;
using Northwind.Odata.Api.Client.Odata.Employees;
using Northwind.Odata.Api.Client.Models;

// API requires no authentication, so use the anonymous
// authentication provider
var authProvider = new AnonymousAuthenticationProvider();
// Create request adapter using the HttpClient-based implementation
var adapter = new HttpClientRequestAdapter(authProvider);
// Create the API client
var client = new  NorthwindClient(adapter);

try
{
  EmployeesRequestBuilder employeesRequestBuilder = client.Odata.Employees;
    var employees = await employeesRequestBuilder.GetAsync();

    Console.WriteLine($"Retrieved {employees.Value?.Count} employees.");
    employeesRequestBuilder.PostAsync(new Employee
    {
        FirstName = "John",
        LastName = "Doe",
        Title = "Software Engineer",
        TitleOfCourtesy = "Mr.",
        BirthDate = DateTimeOffset.Now.AddYears(-30),
        HireDate = DateTimeOffset.Now,
        Address = "123 Main St",
        City = "Anytown",
        Region = "CA",
        PostalCode = "12345",
        Country = "USA",
        HomePhone = "555-1234",
        Extension = "123",
        Notes = "New employee",
        PhotoPath = "/photos/johndoe.jpg"

    });
    employees = await employeesRequestBuilder.GetAsync();

    Console.WriteLine($"Retrieved {employees.Value?.Count} employees.");

    //// GET /posts/{id}
    //var specificPostId = 5;
    //var specificPost = await client.Posts[specificPostId].GetAsync();
    //Console.WriteLine($"Retrieved post - ID: {specificPost?.Id}, Title: {specificPost?.Title}, Body: {specificPost?.Body}");

    //// POST /posts
    //var newPost = new Post
    //{
    //    UserId = 42,
    //    Title = "Testing Kiota-generated API client",
    //    Body = "Hello world!"
    //};

    //var createdPost = await client.Posts.PostAsync(newPost);
    //Console.WriteLine($"Created new post with ID: {createdPost?.Id}");

    //// PATCH /posts/{id}
    //var update = new Post
    //{
    //    // Only update title
    //    Title = "Updated title"
    //};

    //var updatedPost = await client.Posts[specificPostId].PatchAsync(update);
    //Console.WriteLine($"Updated post - ID: {updatedPost?.Id}, Title: {updatedPost?.Title}, Body: {updatedPost?.Body}");

    //// DELETE /posts/{id}
    //await client.Posts[specificPostId].DeleteAsync();
}
catch (Exception ex)
{
    Console.WriteLine($"ERROR: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
}