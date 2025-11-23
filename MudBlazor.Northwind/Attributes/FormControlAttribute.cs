using MudBlazor.Northwind.Data;
using System.Collections.Generic;
using System.Text.Json;

namespace MudBlazor.Northwind.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormControlAttribute : Attribute
    {
        public FormControlAttribute(FormControlType type, string? properties = null, string customQuery = "")
        {
            Type = type;
            //IsRequired = isRequired;
            //IsDisabled = isDisabled;
            CustomQuery = customQuery;
            //if (properties != null)
            //{
            //    Properties = JsonSerializer.Deserialize<Dictionary<string, string>>(properties);
            //}
        }
        public Dictionary<string, string>? Properties { get; private set; }

        public FormControlType Type { get; private set; }
        //public bool IsRequired { get; private set; }
        //public bool IsDisabled { get; private set; }
        public string CustomQuery { get; private set; }
    }
}
