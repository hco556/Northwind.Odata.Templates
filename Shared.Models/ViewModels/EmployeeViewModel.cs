using Shared.Models.Attributes;
using Shared.Models.Data;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models.Shared.ViewModels;

public class EmployeeViewModel
{
    [DisplayName("Employee Id")]
    [FormControl(FormControlType.Numeric,true,true)]
    public int EmployeeId { get; set; }
    [DisplayName("Last Name")]
    [FormControl(FormControlType.Text, true, false)]
    public string LastName { get; set; } = null!;
    [DisplayName("First Name")]
    [FormControl(FormControlType.Text, true, false)]
    public string FirstName { get; set; } = null!;
    [DisplayName("Title")]
    [FormControl(FormControlType.Text, false, false)]
    public string? Title { get; set; }

    [DisplayName("PassWord")]
    [FormControl(FormControlType.Password, false, false)]
    [Required] // optional: enforce non-empty
    [PasswordComplexity] // uses defaults: 12 chars, 2 upper, 2 lower, 2 special
    public string Password { get; set; } = null!;

    [DisplayName("Title Of Courtesy")]
    [FormControl(FormControlType.Text, false, false)]
    public string? TitleOfCourtesy { get; set; }

    [DisplayName("Birth Date")]
    [NotInFuture]
    [FormControl(FormControlType.Datetime, true, false)]
    public DateTime? BirthDate { get; set; }

    [DisplayName("Hire Date")]
    [NotInFuture]
    [FormControl(FormControlType.Datetime, false, false)]
    public DateTime? HireDate { get; set; }

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
    [FormControl(FormControlType.DropDownMultiSelect, false, false)]
    public virtual List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();

    [DisplayName("Reports To")]
    [FormControl(FormControlType.DropDownSingleSelect, false, false)]
    public virtual EmployeeViewModel? ReportsToEmployee { get; set; }

    public virtual ICollection<Territory> Territories { get; set; } = new List<Territory>();

    public override string ToString()
    {
        return  FirstName + " " + LastName;
    }
}
