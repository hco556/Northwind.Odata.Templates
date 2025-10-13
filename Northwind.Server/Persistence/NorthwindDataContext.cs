using Microsoft.EntityFrameworkCore;
using Northwind.Server.Domain;

namespace Northwind.Server.Persistence;

public class NorthwindDataContext(DbContextOptions<NorthwindDataContext> options) : DbContext(options)
{
    public DbSet<Employees> Employees { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}

    
