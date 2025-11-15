using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using MudBlazor.Northwind.Constants;
using MudBlazor.Northwind.Mappers.Employee;
using MudBlazor.Northwind.ViewModels;
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
        public KiotaRequestBuilder(NorthwindClient client)
        {
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
        public async Task<List<EmployeeViewModel>> GetEmployeesExcludingId(int id)
        {
            EmployeesRequestBuilder employeesRequestBuilder = _client.Odata.Employees;
            var employeeCollectionResponse = await employeesRequestBuilder.GetAsync(rc =>
            {
                // filter out the given id
                rc.QueryParameters.Filter = $"EmployeeId ne {id}";
            });

            if (employeeCollectionResponse?.Value == null)
                return new List<EmployeeViewModel>();

            // Map OData client models to view models
            var employeesViewmodels = EmployeeMapper.MapEmployeesToViewModels(employeeCollectionResponse.Value);
            return employeesViewmodels ?? new List<EmployeeViewModel>();
        }
        public async Task<List<EmployeeViewModel>> GetAllEmployees()
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
                return employeesViewmodels ?? new List<EmployeeViewModel>();
            }
        }


        public async Task<List<OrderViewModel>> GetOrdersForEmployee(int id)
        {
            var ordersRequestBuilder = _client.Odata.Orders;
            var ordersCollectionResponse = await ordersRequestBuilder.GetAsync(rc =>
            {
                rc.QueryParameters.Filter = $"employeeId eq {id}";
            });

            var result = new List<OrderViewModel>();

            if (ordersCollectionResponse?.Value == null)
                return result;

            foreach (var o in ordersCollectionResponse.Value)
            {
                result.Add(new OrderViewModel
                {
                    OrderId = o.OrderId ?? 0,
                    CustomerId = o.CustomerId,
                    EmployeeId = o.EmployeeId,
                    OrderDate = o.OrderDate?.UtcDateTime
                });
            }

            return result;
        }
    }
}
