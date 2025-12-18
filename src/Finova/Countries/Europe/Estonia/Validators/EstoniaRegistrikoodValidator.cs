using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Estonia.Validators;

/// <summary>
/// Validator for Estonia Registrikood (Commercial Register Code).
/// </summary>
public partial class EstoniaRegistrikoodValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex RegistrikoodRegex();

    public string CountryCode => "EE";

    public ValidationResult Validate(string? number)
    {
        return ValidateRegistrikood(number);
    }



    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateRegistrikood(string? registrikood)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(registrikood, "EE");

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.RegistrikoodCannotBeEmpty);
        }

        if (!RegistrikoodRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidEstoniaRegistrikoodFormat);
        }

        // Checksum Validation (Double Pass)
        // Pass 1 Weights: 1, 2, 3, 4, 5, 6, 7
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7 };
        var digits = normalized.AsSpan(0, 7);
        int sum1 = ChecksumHelper.CalculateWeightedSum(digits.ToString(), weights1);
        int remainder1 = sum1 % 11;
        int checkDigit;

        if (remainder1 < 10)
        {
            checkDigit = remainder1;
        }
        else
        {
            // Pass 2 Weights: 3, 4, 5, 6, 7, 8, 9
            int[] weights2 = { 3, 4, 5, 6, 7, 8, 9 };
            int sum2 = ChecksumHelper.CalculateWeightedSum(digits.ToString(), weights2);
            int remainder2 = sum2 % 11;

            if (remainder2 < 10)
            {
                checkDigit = remainder2;
            }
            else
            {
                checkDigit = 0;
            }
        }

        int lastDigit = normalized[7] - '0';

        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidEstoniaRegistrikoodChecksum);
        }

        return ValidationResult.Success();
    }



    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "EE");
    }
}
