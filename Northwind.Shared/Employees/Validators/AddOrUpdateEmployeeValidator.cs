using FluentValidation;
using Northwind.Shared.Constants;
using Northwind.Shared.Employees.Commands;

namespace Northwind.Shared.Employees.Validators;

public class AddOrUpdateEmployeeValidator : AbstractValidator<AddOrUpdateEmployeeCommand>
{
    public AddOrUpdateEmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty()
            .WithMessage("The first name is required")
            .MaximumLength(MaxLengths.Employees.FirstName)
            .WithMessage("The first name must be less than {MaxLength} characters");
        
        RuleFor(x => x.LastName).NotEmpty()
            .WithMessage("The last name is required")
            .MaximumLength(MaxLengths.Employees.LastName)
            .WithMessage("The last name must be less than {MaxLength} characters");
        
        RuleFor(x => x.Title).NotEmpty()
            .WithMessage("The title is required")
            .MaximumLength(MaxLengths.Employees.Title)
            .WithMessage("The title must be less than {MaxLength} characters");

        RuleFor(x => x.Password).NotEmpty()
            .Matches(@"^(?=(.*[A-Z]){2,})(?=(.*[a-z]){2,})(?=(.*[^A-Za-z0-9]){2,}).{12,}$")
            .WithMessage("Password must be at least 12 characters long and contain at least 2 uppercase, 2 lowercase and 2 special characters.");
    }
}
