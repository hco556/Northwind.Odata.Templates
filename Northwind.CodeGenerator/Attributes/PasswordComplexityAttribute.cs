using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Northwind.CodeGenerator.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public class PasswordComplexityAttribute : ValidationAttribute
{
    private const string DefaultErrorMessage = "Password must be at least {0} characters long and contain at least {1} uppercase, {2} lowercase and {3} special characters.";
    private readonly int _minLength;
    private readonly int _minUpper;
    private readonly int _minLower;
    private readonly int _minSpecial;

    public PasswordComplexityAttribute(int minLength = 12, int minUpper = 2, int minLower = 2, int minSpecial = 2)
        : base(DefaultErrorMessage)
    {
        _minLength = minLength;
        _minUpper = minUpper;
        _minLower = minLower;
        _minSpecial = minSpecial;
    }

    public override string FormatErrorMessage(string name) =>
        string.Format(ErrorMessageString ?? DefaultErrorMessage, _minLength, _minUpper, _minLower, _minSpecial);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var password = value as string;
        if (string.IsNullOrEmpty(password))
            return ValidationResult.Success; // use [Required] to enforce presence

        // Lookahead-based pattern that enforces counts and min length:
        var pattern = $@"^(?=(.*[A-Z]){{{_minUpper},}})(?=(.*[a-z]){{{_minLower},}})(?=(.*[^A-Za-z0-9]){{{_minSpecial},}}).{{{_minLength},}}$";

        if (Regex.IsMatch(password, pattern))
            return ValidationResult.Success;

        return new ValidationResult(FormatErrorMessage(validationContext?.DisplayName ?? validationContext?.MemberName ?? "Password"));
    }
}
