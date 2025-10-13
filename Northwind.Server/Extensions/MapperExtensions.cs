using Northwind.Server.Domain;
using Northwind.Shared.Employees;
using Northwind.Shared.Employees.Commands;
using Riok.Mapperly.Abstractions;

namespace Northwind.Server.Extensions;

[Mapper]
public static partial class MapperExtensions
{
    public static partial IQueryable<EmployeesDto> ProjectToDtos(this IQueryable<Employees> queryable);

    public static partial Employees MapToNewEmployee(this AddOrUpdateEmployeeCommand command);

    public static partial void MapToExistingEmployee(this AddOrUpdateEmployeeCommand command,
        Employees existingEmployee);

}
