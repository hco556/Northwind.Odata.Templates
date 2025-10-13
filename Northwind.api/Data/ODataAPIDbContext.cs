using Microsoft.EntityFrameworkCore;
using Shared.Models.TestModels;


namespace Northwind.api.Data
{
    public class ODataAPIDbContext : DbContext
    {
        public ODataAPIDbContext(DbContextOptions<ODataAPIDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
   
}
