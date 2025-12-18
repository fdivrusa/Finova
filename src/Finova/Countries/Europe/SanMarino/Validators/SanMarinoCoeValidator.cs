using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.SanMarino.Validators;

public partial class SanMarinoCoeValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{5}$")]
    private static partial Regex CoeRegex();

    public string CountryCode => "SM";
    public static EnterpriseNumberType Type => EnterpriseNumberType.SanMarinoCoe;

    public ValidationResult Validate(string? input) => ValidateCoe(input);

    public string? Parse(string? number) => Normalize(number);

    public static ValidationResult ValidateCoe(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.Replace(" ", "");

        if (!CoeRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSanMarinoCoeFormat);
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
