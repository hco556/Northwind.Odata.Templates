using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Northwind.Server.Domain;
using Northwind.Shared.Constants;

namespace Northwind.Server.Persistence.Configurations;

public class NorthwindConfiguration: IEntityTypeConfiguration<Employees>
{
    public void Configure(EntityTypeBuilder<Employees> builder)
    {
        builder.HasKey(x => x.EmployeeId);
        
        builder.Property(x => x.FirstName)
            .IsRequired().HasMaxLength(MaxLengths.Employees.FirstName);
        
        builder.Property(x => x.LastName)
            .IsRequired().HasMaxLength(MaxLengths.Employees.LastName);

        builder.Property(x => x.Title)
            .IsRequired().HasMaxLength(MaxLengths.Employees.Title);    

        builder.ToTable("Employees");
    }
}