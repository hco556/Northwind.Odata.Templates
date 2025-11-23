namespace MudBlazor.Northwind.Data
{
    public enum ProductType
    {
        Standard,
        Professional
    }

    public enum FormControlType
    {
        Text,
        Textarea,
        Datetime,
        Checkbox,
        Numeric,
        DropDownSingleSelect,
        DropDownMultiSelect,
        Password
    }
    public class FormControlProperties
   {
        public Dictionary<string, string>? Properties { get; set; }
}
}
