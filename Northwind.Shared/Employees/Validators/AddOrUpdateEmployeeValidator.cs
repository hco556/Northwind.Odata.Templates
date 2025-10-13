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
    }
}