using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

/// <summary>
/// Validator for Czech Republic IČO (Identifikační číslo osoby).
/// </summary>
public partial class CzechRepublicIcoValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex IcoRegex();

    public string CountryCode => "CZ";

    public ValidationResult Validate(string? number)
    {
        return ValidateIco(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateIco(string? ico)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(ico, "CZ");

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.CzechRepublicIcoEmpty);
        }

        if (!IcoRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CzechRepublicIcoInvalidFormat);
        }

        // Checksum Validation
        // Weights: 8, 7, 6, 5, 4, 3, 2
        int[] weights = { 8, 7, 6, 5, 4, 3, 2 };
        int sum = ChecksumHelper.CalculateWeightedSum(normalized.AsSpan(0, 7).ToString(), weights);

        int remainder = sum % 11;
        int checkDigit = (11 - remainder) % 10;

        int lastDigit = normalized[7] - '0';

        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.CzechRepublicIcoInvalidChecksum);
        }

        return ValidationResult.Success();
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "CZ");
    }
}
