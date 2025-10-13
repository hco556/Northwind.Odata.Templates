using System;
using Northwind.Shared.Employees;
using Northwind.Shared.Employees.Commands;
using Riok.Mapperly.Abstractions;

namespace Northwind.Client.Extensions;

[Mapper]
public static partial class MapperExtension
{
    public static partial AddOrUpdateEmployeeCommand MapToCommand(this EmployeesDto employees);

}
