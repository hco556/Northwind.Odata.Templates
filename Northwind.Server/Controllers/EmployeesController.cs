using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Northwind.Server.Extensions;
using Northwind.Server.Persistence;
using Northwind.Shared.Constants;
using Northwind.Shared.Employees;
using Northwind.Shared.Employees.Commands;

namespace Northwind.Server.Controllers;

[ApiController]
public class EmployeesController(NorthwindDataContext northwindDataContext) : Controller
{
    [HttpGet(Routes.Api.Employees.GetAll)]
    public async Task<ActionResult<List<EmployeesDto>>> GetAll()
    {
        var employees = await northwindDataContext.Employees
        .ProjectToDtos()
        .ToListAsync();
        return Ok(employees);
    }

    [HttpGet(Routes.Api.Employees.GetById)]
    public async Task<ActionResult<EmployeesDto?>> GetById(int id)
    {
        var employee = await northwindDataContext.Employees
        .ProjectToDtos()
        .FirstOrDefaultAsync(x => x.EmployeeId == id);

        if (employee == null)
            return NotFound();

        return Ok(employee);
    }

    [HttpPost(Routes.Api.Employees.Add)]
    public async Task<ActionResult> Add(AddOrUpdateEmployeeCommand command)
    {
        var employee = command.MapToNewEmployee();

        // Ensure EmployeeId is not set
        employee.EmployeeId = 0;


        northwindDataContext.Employees.Add(employee);
        await northwindDataContext.SaveChangesAsync();

        return Created();
    }

    [HttpPut(Routes.Api.Employees.Update)]
    public async Task<ActionResult> Update(AddOrUpdateEmployeeCommand command)
    {
        var existingEmployee = await northwindDataContext.Employees.FindAsync(command.EmployeeId);

        if (existingEmployee == null)
            return NotFound();

        command.MapToExistingEmployee(existingEmployee);

        await northwindDataContext.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete(Routes.Api.Employees.Delete)]
    public async Task<ActionResult> Delete(int id)
    {
        var existingEmployee = await northwindDataContext.Employees.FindAsync(id);

        if (existingEmployee == null)
            return NotFound();

        northwindDataContext.Employees.Remove(existingEmployee);
        await northwindDataContext.SaveChangesAsync();

        return Ok();
    }
}