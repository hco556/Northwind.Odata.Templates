using Northwind.CodeGenerator.Attributes;
using Northwind.CodeGenerator.Helpers;
using Shared.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Linq;
using System.Net.Security;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using Northwind.CodeGenerator.Generators;
using Northwind.CodeGenerator.ViewModels;
string splitTokensForLabel = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
var baseType = typeof(Shared.Models.Employee);

var types = baseType.Assembly.GetTypes().Where(t => t.IsPublic);// (t => t != baseType && baseType.IsAssignableFrom(t) && t.IsPublic && t.IsInterface);
foreach (var type in types)
{
    Console.WriteLine(type);

}
Employee employee = new Employee()
{
    Territories = new List<Territory>()
};
Dictionary<string,PropertyInfo> PropertyInfoList = new();
List<string> navigationPropertyNames = new();
var Properties = typeof(Employee).GetProperties()
        .ToList();
foreach (var property in Properties)
{

    PropertyInfo propertyInfo = property;
    var hasDisplayNameAttribute = Attribute.IsDefined(baseType.Assembly, typeof(DisplayNameAttribute));
    var isReverseAttribute = Attribute.IsDefined(baseType.Assembly, typeof(InversePropertyAttribute));

    propertyInfo.GetAttribute<KeyAttribute>();
    //propertyInfo.GetValue(employee)
   // PropertyAttributes attributes = propertyInfo.Attributes;
    Type propertyType = propertyInfo.PropertyType;
    //string? label = propertyInfo?.GetDisplayName();
    //FormControlAttribute? formControlAttribute = propertyInfo?.GetAttribute<FormControlAttribute>();
    //var formControlType = formControlAttribute?.Type;
    //var query = formControlAttribute?.CustomQuery;
    string label = Regex.Replace(property.Name, "(?<!^)([A-Z])", " $1");
    if (!isReverseAttribute && !property.Name.Contains("Inverse", StringComparison.OrdinalIgnoreCase))//Don't include the Inverse properties
    {
        PropertyInfoList.Add(property.Name,propertyInfo);
        if (property.Name.Contains("Navigation", StringComparison.OrdinalIgnoreCase))
        {
            navigationPropertyNames.Add(property.Name);
        }
        Console.WriteLine(property.Name + " - Type: " + propertyType + " - label: " + label);
    //Console.WriteLine(property.Name + " - " + propertyType + " - label: " + label?? String.Empty + " - Control: " + formControlType + " - Query: " + query);
    }
}
foreach (var navprop in navigationPropertyNames)
{
    var removeProp = navprop.Replace("Navigation", "", StringComparison.OrdinalIgnoreCase);
    Console.WriteLine("Remove the FK Id Navigation Property " + removeProp);
    PropertyInfoList.Remove(removeProp);
}
var vmClassName = "EmployeeViewModel";
StringBuilder viewModelStringBuilder = new StringBuilder();
viewModelStringBuilder.AppendLine("public class " + vmClassName + "{");
foreach (var kv in PropertyInfoList)
{
    string propName = kv.Key;
    PropertyInfo propInfo = kv.Value;
    string label = Regex.Replace(propName, "(?<!^)([A-Z])", " $1");
    viewModelStringBuilder.AppendLine("     [DisplayName(\"" + label + "\")]");
    string propType = propInfo.PropertyType.ToString();
    string typeString = String.Empty;
    if (propType.Contains("System.Int32"))
        typeString = "int";
    else if (propType.Contains("System.String"))
        typeString = "string";
    else if (propType.Contains("System.Boolean"))
        typeString = "bool";
    else
        typeString = propType;

    bool isCollection = propType.Contains("ICollection");
    bool isNullable = propType.Contains("Nullable");
    string nullableString = isNullable ? "?" : "";

    viewModelStringBuilder.AppendLine("     public " + typeString + nullableString + " " + typeString +  "{ get; set; }");
}
viewModelStringBuilder.AppendLine("}");
Console.WriteLine(viewModelStringBuilder);
Employee employeeTemplate = new Employee();
Type employeeTemplateType = employee.GetType();
Console.WriteLine(employeeTemplateType.Name);
MemberInfo[] members = employeeTemplateType.GetMembers();

foreach (var member in members)
{
    var memberType = member.MemberType;

    if (memberType.ToString() == "Property")
    {

        PropertyInfo? propInfo = employeeTemplateType.GetProperty(member.Name);
        Type propType = propInfo.PropertyType;
        var shortPropTypeName = propType.Name;
        //if (member.Name == "Title" || member.Name == "LastName")
        //    break;
        if (propType.Name.Contains("Nullable") || propType.Name.Contains("ICollection"))
        {
            //System.Nullable`1[[System.Int32,
            var startIdx = propType.FullName.IndexOf("[[")+2;
           // var propTypeName = propType.FullName.Replace("System.Nullable`1[[", "");
            var endIdx = propType.FullName.IndexOf(",");
            shortPropTypeName = propType.FullName.Substring(startIdx, endIdx- startIdx);

        }
        if (propType.Name.Contains("Nullable"))
        {
            shortPropTypeName += "?";
        }
        if (propType.Name.Contains("ICollection"))
        {
           shortPropTypeName = "ICollection<" + shortPropTypeName + ">";
        }

        Console.WriteLine(shortPropTypeName + " " + member.Name);
    }
    
}

var props = typeof(Shared.Models.Employee).GetProperties(BindingFlags.Public | BindingFlags.Instance);
foreach (var p in props)
{
    Console.WriteLine($"{p.Name} ({p.PropertyType.Name}) Nullable: {ReflectionExtensions.IsNullable(p)}");
}
//string employeeViewModelContents = ViewModelGenerator.GenerateViewModelSource(typeof(Shared.Models.Employee), "Generated.ViewModels", "EmployeeViewModel");
//var generatedfilespath = "C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\Generated\\EmployeeViewModel.cs";
//File.WriteAllText(generatedfilespath, employeeViewModelContents);
MudBlazor.Northwind.ViewModels.EmployeeViewModel employeevm = new MudBlazor.Northwind.ViewModels.EmployeeViewModel();
string employeeFormContents = MudFormGenerator.GenerateMudForm(employeevm.GetType(), "MudBlazor.Northwind.Components.Employees", "EmployeeEditFormGen");
var generatedfilesmudformpath = "C:\\Users\\hcopp\\Documents\\Work\\OData\\Northwind.OData.Templates\\MudBlazor.Northwind\\Components\\Pages\\Employees\\EmployeeEditFormGen.razor";
File.WriteAllText(generatedfilesmudformpath, employeeFormContents);
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
