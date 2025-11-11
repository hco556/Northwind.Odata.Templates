using System;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Templates.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class NotInFutureAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "{0} cannot be a future date.";

    public NotInFutureAttribute() : base(DefaultErrorMessage) { }

    public override string FormatErrorMessage(string name) =>
        string.Format(ErrorMessageString ?? DefaultErrorMessage, name);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        // Nulls are allowed — use [Required] when the value must be present.
        if (value is null)
            return ValidationResult.Success;

        if (value is DateTime dt)
        {
            // Compare dates only (ignore time-of-day)
            if (dt.Date <= DateTime.Today)
                return ValidationResult.Success;

            var displayName = validationContext?.DisplayName ?? validationContext?.MemberName ?? "The field";
            return new ValidationResult(FormatErrorMessage(displayName));
        }

        return new ValidationResult("Invalid data type for date validation.");
    }
}