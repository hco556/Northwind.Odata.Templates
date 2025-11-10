
using Shared.Models.Shared.ViewModels;
using OdataClientModels = Northwind.Odata.Api.Client.Models;

namespace MudBlazor.Northwind.Mappers.Employee
{
    public static class EmployeeMapper
    {
        public static List<EmployeeViewModel> MapEmployeesToViewModels(List<OdataClientModels.Employee> employees)
        {
            if (employees is null) throw new ArgumentNullException(nameof(employees));

            var result = new List<EmployeeViewModel>(employees.Count);
            foreach (var employee in employees)
            {
                if (employee is null) continue; // tolerate null entries in the list
                result.Add(MapEmployeeToViewModel(employee));
            }

            return result;
        }
        public static EmployeeViewModel MapEmployeeToViewModel(OdataClientModels.Employee employee)
        {
            if (employee is null) throw new ArgumentNullException(nameof(employee));

            return new EmployeeViewModel
            {
                EmployeeId = employee.EmployeeId ?? 0,
                LastName = employee.LastName ?? string.Empty,
                FirstName = employee.FirstName ?? string.Empty,
                Title = employee.Title,
                TitleOfCourtesy = employee.TitleOfCourtesy,
                BirthDate = employee.BirthDate?.UtcDateTime,
                HireDate = employee.HireDate?.UtcDateTime,
                Address = employee.Address,
                City = employee.City,
                Region = employee.Region,
                PostalCode = employee.PostalCode,
                Country = employee.Country,
                HomePhone = employee.HomePhone,
                Extension = employee.Extension,
                Photo = employee.Photo,
                Notes = employee.Notes,
                ReportsTo = employee.ReportsTo,
                PhotoPath = employee.PhotoPath,
                //InverseReportsToNavigation = employee.InverseReportsToNavigation != null
                //    ? new List<Employee>(employee.InverseReportsToNavigation)
                //    : new List<Employee>(),
                //Orders = employee.Orders != null
                //    ? new List<Order>(employee.Orders)
                //    : new List<Order>(),
                //ReportsToNavigation = employee.ReportsToNavigation,
                //Territories = employee.Territories != null
                //    ? new List<Territory>(employee.Territories)
                //    : new List<Territory>()
            };
        }
    }
}
