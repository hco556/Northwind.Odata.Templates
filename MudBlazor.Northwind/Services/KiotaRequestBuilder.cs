using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MudBlazor.Northwind.Constants;
using MudBlazor.Northwind.Mappers.Employee;
using MudBlazor.Northwind.Shared.ViewModels;
using Northwind.Odata.Api.Client;
using Northwind.Odata.Api.Client.Odata.Employees;
using OdataClientModels = Northwind.Odata.Api.Client.Models;


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
        public async Task<EmployeeViewModel?> GetEmployeeByCustomQuery(string query)
        {
     
            EmployeesRequestBuilder employeesRequestBuilder = _client.Odata.Employees;
            //https://localhost:5000/odata/Employees?%24top=1
            // var employee = await employeesRequestBuilder.WithUrl(ConstantCalls.BaseUrl + query).GetAsync();
            OdataClientModels.Employee? employee = null;
            EmployeeViewModel? employeeViewModel = new EmployeeViewModel();
            var employeeCollectionResponse = await employeesRequestBuilder.GetAsync(rc =>
            {
                rc.QueryParameters.Filter = $"{query}";
            });

            if (employeeCollectionResponse != null)
            {
                employee = employeeCollectionResponse.Value?.FirstOrDefault();
            }
            if (employeeViewModel != null)
            {
                employeeViewModel = EmployeeMapper.MapEmployeeToViewModel(employee);
                return employeeViewModel;
            }
            return null;
        }
        public async Task<EmployeeViewModel?> GetEmployeeById(int Id)
        {
            EmployeesRequestBuilder employeesRequestBuilder = _client.Odata.Employees;
            var employeeCollectionResponse = await employeesRequestBuilder.GetAsync(rc =>
            {
                rc.QueryParameters.Filter = $"EmployeeId eq {Id}";
            });
            //  var employee = await _client.Odata.Employees[Id].GetAsync();
            //  return EmployeeMapper.MapEmployeeToViewModel(employee); ;
            OdataClientModels.Employee? employee = new();
            if (employeeCollectionResponse != null)
            {
                employee = employeeCollectionResponse.Value?.FirstOrDefault();
            }
            if (employee != null)
            {
                EmployeeViewModel? employeeViewModel = EmployeeMapper.MapEmployeeToViewModel(employee);
                return employeeViewModel;
            }
            return null;

        }
        public async Task<EmployeeViewModel?> GetEmployeeByName(string lastName)
        {
           
            EmployeesRequestBuilder employeesRequestBuilder = _client.Odata.Employees;
            //https://localhost:5000/odata/Employees?%24top=50
        //    var employeesFitered = await employeesRequestBuilder.WithUrl(ConstantCalls.BaseUrl + "Employees?$top=1").GetAsync();

            

            var employeeCollectionResponse = await employeesRequestBuilder.GetAsync(rc =>
            {
                rc.QueryParameters.Filter = $"lastName eq '{lastName}'";
            });
            OdataClientModels.Employee? employee = new();
            if (employeeCollectionResponse != null)
            {
                employee = employeeCollectionResponse.Value?.FirstOrDefault();
            }
            if (employee != null)
            {
                EmployeeViewModel? employeeViewModel = EmployeeMapper.MapEmployeeToViewModel(employee);
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
