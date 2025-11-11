using static System.Net.WebRequestMethods;

namespace MudBlazor.Northwind.Constants
{
    public static class ConstantCalls
    {
        public const string BaseUrl = "https://localhost:5000/odata/";
        public const string GetEmployeeManagers = "employees?$filter=Id ne {Id}";
        public const string GetOrdersForEmployee = "orders?$filter=employeeId eq {Id}";
    }
}
