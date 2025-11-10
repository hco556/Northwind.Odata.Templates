using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MudBlazor.Northwind.Mappers.Employee;
using Northwind.Odata.Api.Client;
using Northwind.Odata.Api.Client.Models;
using Northwind.Odata.Api.Client.Odata.Employees;
using Shared.Models.Shared.ViewModels;


namespace MudBlazor.Northwind.Services
{
    public class KiotaRequestBuilder
    {
        //private readonly IAuthenticationProvider authProvider;
        //// Create request adapter using the HttpClient-based implementation
        //private readonly HttpClientRequestAdapter adapter;
        // Create the API client
        private readonly NorthwindClient _client;
        //To Do: Dependency Injection
        public KiotaRequestBuilder(NorthwindClient client) {
            // Create the API client
            _client = client;
       
        }
        public async Task<EmployeeViewModel?> GetEmployeeByName(string lastName)
        {
            Employee employee = null;
            EmployeesRequestBuilder employeesRequestBuilder = _client.Odata.Employees;
            //https://localhost:5000/odata/Employees?%24top=50
            var employeesFitered = await employeesRequestBuilder.WithUrl("https://localhost:5000/odata/Employees?$top=1").GetAsync();

            var employeeCollectionResponse = await employeesRequestBuilder.GetAsync(rc =>
            {
                rc.QueryParameters.Filter = $"lastName eq '{lastName}'";
            });

            if (employeeCollectionResponse != null)
            {
                employee = employeeCollectionResponse.Value.FirstOrDefault();
            }
            if (employee != null)
            {
                EmployeeViewModel employeeViewModel = EmployeeMapper.MapEmployeeToViewModel(employee);
                return employeeViewModel;
            }
            return null;
        }
        public async Task<List<EmployeeViewModel>>GetAllEmployees()
        {
            EmployeesRequestBuilder employeesRequestBuilder = _client.Odata.Employees;
            var employees = await employeesRequestBuilder.GetAsync();
       
            if (employees == null)
            {
                return new List<EmployeeViewModel>();
            }
            else
            {
                var employeesViewmodels = EmployeeMapper.MapEmployeesToViewModels(employees.Value);
                return employeesViewmodels?? new List<EmployeeViewModel>();
            }
        }
    }
}
