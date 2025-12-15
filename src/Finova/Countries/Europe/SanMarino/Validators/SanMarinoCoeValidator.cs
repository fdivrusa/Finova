using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.SanMarino.Validators;

public partial class SanMarinoCoeValidator : IEnterpriseValidator
{
    [GeneratedRegex(@"^\d{5}$")]
    private static partial Regex CoeRegex();

    public string CountryCode => "SM";
    public EnterpriseNumberType Type => EnterpriseNumberType.SanMarinoCoe;

    public ValidationResult Validate(string? number) => ValidateCoe(number);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateCoe(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Empty.");
        }

        var cleaned = number.Replace(" ", "");

        if (!CoeRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid San Marino COE format.");
        }

        return ValidationResult.Success();
    }

    public string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.Replace(" ", "");
        return CoeRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
