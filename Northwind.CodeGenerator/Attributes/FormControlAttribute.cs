using Northwind.CodeGenerator.Data;

namespace Northwind.CodeGenerator.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormControlAttribute : Attribute
    {
        public FormControlAttribute(FormControlType type,string customQuery = "")
        {
            Type = type;
            //IsRequired = isRequired;
            //IsDisabled = isDisabled;
            CustomQuery = customQuery;
        }

        public FormControlType Type { get; private set; }
        //public bool IsRequired { get; private set; }
        //public bool IsDisabled { get; private set; }
        public string CustomQuery { get; private set; }
    }
}
