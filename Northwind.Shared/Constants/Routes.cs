using System.Text.RegularExpressions;

namespace Northwind.Shared.Constants;

public static partial class Routes
{
    [GeneratedRegex("{.*?}")]
    private static partial Regex StringFormatArgsRegex();
    public static string Format(this string template, params object[] args)
    {
        var index = 0;
        var formattedTemplate = StringFormatArgsRegex().Replace(template, _ => $"{{{index++}}}");
        return string.Format(formattedTemplate, args);
    }
    public static class Api
    {
        public static class Employees
        {
            public const string GetAll = "api/employees";
            public const string GetById = "api/employees/{id:int}";
            public const string Add = "api/employees";
            public const string Update = "api/employees";
            public const string Delete = "api/employees/{id:int}";
        }
    }

    public static class Pages
    {
        public const string Home = "/";
        public const string Counter = "/counter";
        public const string Weather = "/weather";

        public static class Employees
        {
            public const string GetAPIDocumentation = "/scalar/v1";
            public const string GetAllEmployees = "/employees";    
            public const string Add = "/employees/add";
            public const string Edit = "/employees/edit/{id:int}";
        }
    }
}
