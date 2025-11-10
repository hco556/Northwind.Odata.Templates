using Shared.Models.Data;

namespace Northwind.Odata.Api.Models
{
    public class PropertySet
    {
        public readonly string PropertyName;
        public readonly Type PropertyType;
        public readonly string PropertyLabel;
        public readonly bool PropertyIsRequired;
        public readonly bool PropertyIsDisabled;
        public readonly FormControlType FormControlType;
        public PropertySet(string propertyName, Type propertyType, FormControlType formControlType, string propertyLabel, bool propertyIsRequired,bool propertyIsDisabled)
        {
            FormControlType = formControlType;
            PropertyName = propertyName;
            PropertyType = propertyType;
            PropertyLabel = propertyLabel;
            PropertyIsRequired = propertyIsRequired;
            PropertyIsDisabled = propertyIsDisabled;
        }
    }
}
