using MudBlazor.Extensions.Options;
using MudBlazor.Northwind.Services;
using Northwind.CodeGenerator.Attributes;
using Northwind.CodeGenerator.Generators;
using Northwind.CodeGenerator.Helpers;
using Northwind.CodeGenerator.ViewModels;
using Shared.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Core.Tokens;

string splitTokensForLabel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
var baseType = typeof(Shared.Models.Employee);

//var types = baseType.Assembly.GetTypes().Where(t => t.IsPublic);// (t => t != baseType && baseType.IsAssignableFrom(t) && t.IsPublic && t.IsInterface);
//foreach (var type in types)
//{
//    Console.WriteLine(type);

//}
Employee employee = new Employee()
{
    Territories = new List<Territory>()
};
//Dictionary<string,PropertyInfo> PropertyInfoList = new();
//List<string> navigationPropertyNames = new();
//var Properties = typeof(Employee).GetProperties()
//        .ToList();
//foreach (var property in Properties)
//{

//    PropertyInfo propertyInfo = property;
//    var hasDisplayNameAttribute = Attribute.IsDefined(baseType.Assembly, typeof(DisplayNameAttribute));
//    var isReverseAttribute = Attribute.IsDefined(baseType.Assembly, typeof(InversePropertyAttribute));

//    propertyInfo.GetAttribute<KeyAttribute>();
//    //propertyInfo.GetValue(employee)
//   // PropertyAttributes attributes = propertyInfo.Attributes;
//    Type propertyType = propertyInfo.PropertyType;
//    //string? label = propertyInfo?.GetDisplayName();
//    //FormControlAttribute? formControlAttribute = propertyInfo?.GetAttribute<FormControlAttribute>();
//    //var formControlType = formControlAttribute?.Type;
//    //var query = formControlAttribute?.CustomQuery;
//    string label = Regex.Replace(property.Name, "(?<!^)([A-Z])", " $1");
//    if (!isReverseAttribute && !property.Name.Contains("Inverse", StringComparison.OrdinalIgnoreCase))//Don't include the Inverse properties
//    {
//        PropertyInfoList.Add(property.Name,propertyInfo);
//        if (property.Name.Contains("Navigation", StringComparison.OrdinalIgnoreCase))
//        {
//            navigationPropertyNames.Add(property.Name);
//        }
//        Console.WriteLine(property.Name + " - Type: " + propertyType + " - label: " + label);
//    //Console.WriteLine(property.Name + " - " + propertyType + " - label: " + label?? String.Empty + " - Control: " + formControlType + " - Query: " + query);
//    }
//}
//foreach (var navprop in navigationPropertyNames)
//{
//    var removeProp = navprop.Replace("Navigation", "", StringComparison.OrdinalIgnoreCase);
//    Console.WriteLine("Remove the FK Id Navigation Property " + removeProp);
//    PropertyInfoList.Remove(removeProp);
//}
//var vmClassName = "EmployeeViewModel";
//StringBuilder viewModelStringBuilder = new StringBuilder();
//viewModelStringBuilder.AppendLine("public class " + vmClassName + "{");
//foreach (var kv in PropertyInfoList)
//{
//    string propName = kv.Key;
//    PropertyInfo propInfo = kv.Value;
//    string label = Regex.Replace(propName, "(?<!^)([A-Z])", " $1");
//    viewModelStringBuilder.AppendLine("     [DisplayName(\"" + label + "\")]");
//    string propType = propInfo.PropertyType.ToString();
//    string typeString = String.Empty;
//    if (propType.Contains("System.Int32"))
//        typeString = "int";
//    else if (propType.Contains("System.String"))
//        typeString = "string";
//    else if (propType.Contains("System.Boolean"))
//        typeString = "bool";
//    else
//        typeString = propType;

//    bool isCollection = propType.Contains("ICollection");
//    bool isNullable = propType.Contains("Nullable");
//    string nullableString = isNullable ? "?" : "";

//    viewModelStringBuilder.AppendLine("     public " + typeString + nullableString + " " + typeString +  "{ get; set; }");
//}
//viewModelStringBuilder.AppendLine("}");
//Console.WriteLine(viewModelStringBuilder);
//Employee employeeTemplate = new Employee();
//Type employeeTemplateType = employee.GetType();
//Console.WriteLine(employeeTemplateType.Name);
//MemberInfo[] members = employeeTemplateType.GetMembers();
//var vmClassName = "EmployeeViewModel";
//StringBuilder viewModelStringBuilder = new StringBuilder();
//viewModelStringBuilder.AppendLine("public class " + vmClassName + "{");
//foreach (var member in members)
//{
//    var memberType = member.MemberType;

//    if (memberType.ToString() == "Property")
//    {

//        PropertyInfo? propInfo = employeeTemplateType.GetProperty(member.Name);
//        Type propType = propInfo.PropertyType;
//        var shortPropTypeName = propType.Name;
//        string label = Regex.Replace(member.Name, "(?<!^)([A-Z])", " $1");
//        viewModelStringBuilder.AppendLine("     [DisplayName(\"" + label + "\")]");
//        //if (member.Name == "Title" || member.Name == "LastName")
//        //    break;
//        if (propType.Name.Contains("Nullable") || propType.Name.Contains("ICollection"))
//        {
//            //System.Nullable`1[[System.Int32,
//            var startIdx = propType.FullName.IndexOf("[[")+2;
//           // var propTypeName = propType.FullName.Replace("System.Nullable`1[[", "");
//            var endIdx = propType.FullName.IndexOf(",");
//            shortPropTypeName = propType.FullName.Substring(startIdx, endIdx- startIdx);

//        }
//        if (propType.Name.Contains("Nullable") || ReflectionExtensions.IsNullable(propInfo))
//        {
//            shortPropTypeName += "?";
//        }
//        if (propType.Name.Contains("ICollection"))
//        {
//            var collectionElementType = ReflectionExtensions.GetCollectionElementType(employeeTemplateType, member.Name);
//            shortPropTypeName = "ICollection<" + collectionElementType + ">";

//        }

//        Console.WriteLine(shortPropTypeName + " " + member.Name);
//        viewModelStringBuilder.AppendLine("     public " + shortPropTypeName + " " + member.Name + "{ get; set; }");
//    }

//}
//viewModelStringBuilder.AppendLine("}");
//Console.WriteLine(viewModelStringBuilder);
var boss =new Employee
{
    EmployeeId = 3,
    FirstName = "Mr",
    LastName = "Manager",
    Title = "Boss",
    TitleOfCourtesy = "Mr.",
    BirthDate = DateTime.Now.AddYears(-45),
    HireDate = DateTime.Now,
    Address = "123 Main St",
    City = "Anytown",
    Region = "CA",
    PostalCode = "12345",
    Country = "USA",
    HomePhone = "555-1234",
    Extension = "123",
    Notes = "Boss",
    PhotoPath = "/photos/boss.jpg"
};
Employee item = new Employee
{
    EmployeeId = 1,
    FirstName = "Jane",
    LastName = "Doe",
    Title = "Software Engineer",
    TitleOfCourtesy = "Mr.",
    BirthDate = DateTime.Now.AddYears(-25),
    HireDate = DateTime.Now,
    Address = "123 Main St",
    City = "Anytown",
    Region = "CA",
    PostalCode = "12345",
    Country = "USA",
    HomePhone = "555-1234",
    Extension = "123",
    Notes = "New employee",
    PhotoPath = "/photos/janedoe.jpg"

};


Order order1 = new Order { CustomerId = "Customer1", EmployeeId = 1, OrderDate = DateTime.Now, OrderId = 1 };
Order order2 = new Order { CustomerId = "Customer2", EmployeeId = 1, OrderDate = DateTime.Now, OrderId = 2 };
item.Orders = new List<Order> { order1, order2 };
item.ReportsTo = boss.EmployeeId;
item.ReportsToNavigation = boss;
string vmcontent = MudFormGenerator.GenerateViewModel<Employee>(item);
Console.WriteLine(vmcontent);
//var props = typeof(Shared.Models.Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
//foreach (var p in props)
//{
//    Console.WriteLine($"{p.Name} ({p.PropertyType.Name}) Nullable: {ReflectionExtensions.IsNullable(p)}");
//}
var empVMFormModel = new MudBlazor.Northwind.ViewModels.EmployeeViewModel();
//var datePicker = MudControlFactory.CreateControl<MudBlazor.MudDatePicker>(
//    new Dictionary<string, object?>
//    {
//        { "Mask", "00/00/0000" },
//        { "IsDateDisabledFunc", (Func<DateTime, bool>)(d => d.DayOfWeek == DayOfWeek.Sunday) },
//        { "AdditionalDateClassesFunc", (Func<DateTime, string?>)(d => d.Day == 1 ? "highlight" : null) },
//        { "@bind-Date", empVMFormModel.BirthDate},
//        { "Placeholder", "Select a date" }
//    }
//);
//var numericField = MudControlFactory.CreateControl<MudBlazor.MudNumericField<int>>(
//    new Dictionary<string, object?>
//    {
//        { "Label", "Employee Id" },
//        { "Min", 0 },
//        { "Max", 120 },
//        { "@bind-Value",empVMFormModel},
//        { "Required", true },
//        { "Value", empVMFormModel.EmployeeId },
//        { "ValueChanged", (Action<int>)(val => empVMFormModel.EmployeeId = (int)val) },
//        { "ValueExpression", (System.Linq.Expressions.Expression<Func<int>>)(() => empVMFormModel.EmployeeId) }
//    }
//);

//var textField = MudControlFactory.CreateControl<MudBlazor.MudTextField<string>>(
//    new Dictionary<string, object?>
//    {
//        { "Label", "Employee ID" },
//        { "Required", true },
//        { "Value", empVMFormModel.EmployeeId },
//        { "ValueChanged", (Action<string>)(val => empVMFormModel.LastName = val ?? String.Empty) },
//        { "ValueExpression", (System.Linq.Expressions.Expression<Func<string>>)(() => empVMFormModel.LastName) }
//    }
//);
//var singleSelect = MudControlFactory.CreateControl<MudBlazor.Extensions.Components.MudExSelect<EmployeeViewModel>>(
//    new Dictionary<string, object?>
//    {
//        { "MultiSelection", false },
//        { "ItemCollection", new List<EmployeeViewModel> {  } },
//        { "Value", empVMFormModel.ReportsTo },
//        { "ValueChanged", (Action<EmployeeViewModel>)(val => Console.WriteLine($"Selected: {val}")) },
//        { "SearchBox", true },
//        { "SearchBoxVariant", MudBlazor.Variant.Outlined },
//        { "Color", MudBlazor.Color.Primary },
//        { "SelectAll", false }
//    }
//);
//var multiSelect = MudControlFactory.CreateControl<MudBlazor.Extensions.Components.MudExSelect<OrderViewModel>>(
//    new Dictionary<string, object?>
//    {
//        { "MultiSelection", true },
//        { "ItemCollection", new List<OrderViewModel> { } },
//        { "SelectedValues", new HashSet<OrderViewModel> {  } },
//        { "PopOverAnimation", AnimationType.Pulse },
//        { "Label", "Technologies" }
//    }
//);



//// CheckBox
//var activeCheckBox = MudControlFactoryUsage.CreateCheckBoxFromModel(empVMFormModel, nameof(MudBlazor.Northwind.ViewModels.EmployeeViewModel.IsActive));

//// ExSelect (single selection)
//var departmentSelect = MudControlFactoryUsage.CreateExSelectFromModel<MudBlazor.Northwind.ViewModels.EmployeeViewModel, EmployeeViewModel>(
//    empVMFormModel,
//    nameof(MudBlazor.Northwind.ViewModels.EmployeeViewModel.ReportsTo),
//    new List<EmployeeViewModel>() { },
//    multiSelection: false
//);

//// DatePicker
//var hireDatePicker = MudControlFactoryUsage.CreateDatePickerFromModel(empVMFormModel, nameof(EmployeeViewModel.HireDate));

//string employeeViewModelContents = ViewModelGenerator.GenerateViewModelSource(typeof(Shared.Models.Employee), "Generated.ViewModels", "EmployeeViewModel");
//var generatedfilespath = "C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\Generated\\EmployeeViewModel.cs";
//File.WriteAllText(generatedfilespath, employeeViewModelContents);
MudBlazor.Northwind.ViewModels.EmployeeViewModel employeevm = new MudBlazor.Northwind.ViewModels.EmployeeViewModel();
string employeeFormContents = MudFormGenerator.GenerateMudForm(empVMFormModel.GetType(), "MudBlazor.Northwind.Components.Employees", "EmployeeEditFormGen");
var generatedfilesmudformpath = "C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\Components\\Pages\\Employees\\EmployeeEditFormGen.razor";
File.WriteAllText(generatedfilesmudformpath, employeeFormContents);
StringBuilder sb = new StringBuilder(File.ReadAllText("C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\Shared.Models\\Employee.cs"));
//Console.WriteLine(employee.Territories.GetType().Name);
//var employeeVM = new EmployeeViewModel();
//Properties = typeof(EmployeeViewModel).GetProperties()
//        .Where(p => Attribute.IsDefined(p, typeof(DisplayNameAttribute)))
//        .ToList();
//var propertySets = new List<PropertySet>();
//foreach (var property in Properties)
//{
//    PropertyInfo Property = property;
//    object PropertyValue = Property?.GetValue(employeeVM);
//    string label = Property?.GetDisplayName();
//    Type PropertyType = Property?.PropertyType;
//    FormControlAttribute? formControlAttribute = Property.GetAttribute<FormControlAttribute>();
//    var formControlType = formControlAttribute.Text;
//    var isDisabled = false;
//    var isRequired = false;
//    if (formControlAttribute != null)
//    {
//        formControlType = formControlAttribute.Type;
//        //isRequired = formControlAttribute.IsRequired;
//        //isDisabled = formControlAttribute.IsDisabled;
//    }

//    var propertySet = new PropertySet(property.Name, PropertyType, formControlType, label, isRequired, isDisabled);


//    propertySets.Add(propertySet);
//}
;

//var rt = new RuntimeTextTemplate
//{
//    Session = new Dictionary<string, object>()
//};
//rt.Session["Count"] = 7;
//rt.Initialize();
//Console.WriteLine(rt.TransformText());
//var contentsEmployee = File.ReadAllText("C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\ViewModels\\EmployeeViewModel.cs");
//var contentsOrder = File.ReadAllText("C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\ViewModels\\EmployeeViewModel.cs");
//string path = "C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\MyFile.tt";
//string textToAppend = "This is new content.\n";

//File.AppendAllText(path, textToAppend);
