


using Northwind.Odata.Api.Data;
using Shared.Models;

namespace Northwind.OData.Api.Helpers
{
    internal static class ODataAPIDbHelper
    {
        public static void SeedDb(NorthwindContext db)
        {
            if (!db.Employees.Any()) db.Employees.Add(
                new Employee
                {
                    FirstName = "Jane",
                    LastName = "Doe",
                    Title = "Software Engineer",
                    TitleOfCourtesy = "Mr.",
                    BirthDate = DateTime.Now.AddYears(-25),
                    HireDate = DateTime.Now,
                    Address = "123 Main St",
                    City = "Anytown",
                    Region = "CA",
                    PostalCode = "12345",
                    Country = "USA",
                    HomePhone = "555-1234",
                    Extension = "123",
                    Notes = "New employee",
                    PhotoPath = "/photos/janedoe.jpg"

                });
            //if (!db.Customers.Any())
            //{
            //    db.Add(new Customer
            //    {
            //        CustomerId = 1,
            //        ContactName = "Sue",
            //        Country
            //    });

            //    db.Add(new Customer
            //    {
            //        Id = 2,
            //        Name = "Joe",
            //        CustomerType = CustomerType.Wholesale,
            //        CreditLimit = 5100,
            //        CustomerSince = new DateTime(2022, 12, 12)
            //    });

               db.SaveChanges();
            //}
        }
    }
}
