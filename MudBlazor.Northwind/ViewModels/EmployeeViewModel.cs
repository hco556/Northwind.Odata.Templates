
using MudBlazor.Northwind.Attributes;
using MudBlazor.Northwind.Constants;
using MudBlazor.Northwind.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace MudBlazor.Northwind.ViewModels;
//Start-Template//
public class EmployeeViewModel
{
    [DisplayName("Employee Id")]
    [ReadOnly(true)]
    [FormControl(FormControlType.Numeric)]
    public int EmployeeId { get; set; }


    [Range(0, 999999.99)]
    public decimal? Salary { get; set; }

    public DateTime? HireDate { get; set; }

    public bool IsActive { get; set; }

    [StringLength(20, MinimumLength = 3)]
    [DisplayName("Date Created")]
    [ReadOnly(true)]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [DisplayName("Last Name")]
    [FormControl(FormControlType.Text)]
    [Required(ErrorMessage ="Last Name is required")]
    public string LastName { get; set; } = null!;
    [DisplayName("First Name")]
    [Required(ErrorMessage ="First Name is required")]
    [FormControl(FormControlType.Text)]
    public string FirstName { get; set; } = null!;
    [DisplayName("Title")]
    [FormControl(FormControlType.Text)]
    public string? Title { get; set; }

    [DisplayName("PassWord")]
    [FormControl(FormControlType.Password)]
    [Required(ErrorMessage ="Password is Required")] // optional: enforce non-empty
    [PasswordComplexity] // uses defaults: 12 chars, 2 upper, 2 lower, 2 special
    public string Password { get; set; } = null!;

    [DisplayName("Title Of Courtesy")]
    [FormControl(FormControlType.Text)]
    public string? TitleOfCourtesy { get; set; }

    [DisplayName("Birth Date")]
    [NotInFuture]
    [FormControl(FormControlType.Datetime)]
    public DateTime? BirthDate { get; set; }


    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? Country { get; set; }

    public string? HomePhone { get; set; }

    public string? Extension { get; set; }

    public byte[]? Photo { get; set; }

    public string? Notes { get; set; }

    //public int? ReportsTo { get; set; }

    public string? PhotoPath { get; set; }


    [DisplayName("Orders for Employee")]
    [FormControl(FormControlType.DropDownMultiSelect)]
    public virtual List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();

    [DisplayName("Reports To")]
    [FormControl(FormControlType.DropDownSingleSelect,ConstantCalls.GetEmployeeManagers)]
    public virtual EmployeeViewModel? ReportsTo { get; set; }

   // public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();

    public override string ToString()
    {
        return  FirstName + " " + LastName;
    }
}
//End-Template//
