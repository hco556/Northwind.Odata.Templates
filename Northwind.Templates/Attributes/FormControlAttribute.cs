using Northwind.Templates.Data;
using System;

namespace Northwind.Templates.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormControlAttribute : Attribute
    {
        public FormControlAttribute(FormControlType type, bool isRequired = false, bool isDisabled=false)
        {
            Type = type;
            IsRequired = isRequired;
            IsDisabled = isDisabled;
        }

        public FormControlType Type { get; private set; }
        public bool IsRequired { get; private set; }
        public bool IsDisabled { get; private set; }
    }
}
