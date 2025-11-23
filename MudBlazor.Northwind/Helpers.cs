using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace MudBlazor.Northwind
{
    public static class Helpers
    {
        public static T? GetAttribute<T>(this PropertyInfo property) where T : Attribute
        {
            return property.GetCustomAttribute<T>();
        }

        public static string? GetDisplayName(this PropertyInfo property)
        {
            return property.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName;
        }
    }
}
