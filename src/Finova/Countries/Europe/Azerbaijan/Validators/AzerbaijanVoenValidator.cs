using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Azerbaijan.Validators;

/// <summary>
/// Validator for Azerbaijan VÖEN (Tax ID).
/// Wraps the existing AzerbaijanVatValidator and adds additional checks.
/// </summary>
public class AzerbaijanVoenValidator : ITaxIdValidator
{
    public string CountryCode => "AZ";

    public ValidationResult Validate(string? number)
    {
        return ValidateVoen(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateVoen(string? voen)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(voen, "AZ");

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Additional check for Azerbaijan VOEN: Not all digits same
        if (normalized.Distinct().Count() == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidAzerbaijanVoenFormatSameDigits);
        }

        return AzerbaijanVatValidator.Validate(normalized);
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "AZ");
    }
}
