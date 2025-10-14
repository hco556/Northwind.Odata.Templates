using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Generic;
//
// Generated Code - do not edit
//
using Shared.Models;

namespace T4
{
    public class GenTest
    {
       private List<Type> GetTypes() {
            return new List<Type> { typeof(Employee) };
        }
        public void GenMethodTest()
        { 
    var sb = new StringBuilder();

    var types = GetTypes().Where(a => a.Name == "Employee");
    foreach(var t in types)
    {
        var parameters = t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var propertyList = string.Empty;
        foreach(var p in parameters)
        {
            if (propertyList.Length > 0)
            {
                propertyList += ", ";
            }
            propertyList += $"{p.Name}={{{p.Name}}}";
        }
    }
}


    public override string ToString()
    {
        return $"<#=t.Name#>: <#=propertyList#>";
    }
}
}
}

