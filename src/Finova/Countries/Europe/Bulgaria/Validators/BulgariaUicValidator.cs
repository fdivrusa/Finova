using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Bulgaria.Validators;

/// <summary>
/// Validator for Bulgaria UIC / EIK (Unified Identity Code).
/// Supports 9 digits (Legal Entities) and 13 digits (Branches/Sole Traders).
/// </summary>
public partial class BulgariaUicValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^(\d{9}|\d{13})$")]
    private static partial Regex FormatRegex();

    public string CountryCode => "BG";

    public ValidationResult Validate(string? number)
    {
        return ValidateUic(number);
    }

    public string? Parse(string? number)
    {
        return EnterpriseNumberNormalizer.Normalize(number, CountryCode);
    }

    public static ValidationResult ValidateUic(string? uic)
    {
        var normalized = EnterpriseNumberNormalizer.Normalize(uic, "BG");

        if (string.IsNullOrWhiteSpace(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.BulgariaUicEmpty);
        }

        if (!FormatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BulgariaUicInvalidFormat);
        }

        if (normalized.Length == 9)
        {
            return Validate9DigitUic(normalized);
        }
        else
        {
            return Validate13DigitUic(normalized);
        }
    }

    private static ValidationResult Validate9DigitUic(string uic)
    {
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (uic[i] - '0') * weights1[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder < 10)
        {
            checkDigit = remainder;
        }
        else
        {
            int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 10 };
            sum = 0;
            for (int i = 0; i < 8; i++)
            {
                sum += (uic[i] - '0') * weights2[i];
            }
            remainder = sum % 11;
            checkDigit = remainder < 10 ? remainder : 0;
        }

        if (checkDigit != (uic[8] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.BulgariaUicInvalidChecksum9);
        }

        return ValidationResult.Success();
    }

    private static ValidationResult Validate13DigitUic(string uic)
    {
        // First 9 digits must be a valid UIC
        var baseUicResult = Validate9DigitUic(uic.Substring(0, 9));
        if (!baseUicResult.IsValid)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.BulgariaUicInvalidChecksumBase);
        }

        int[] weights1 = { 2, 7, 3, 5 };
        int sum = 0;
        for (int i = 0; i < 4; i++)
        {
            sum += (uic[8 + i] - '0') * weights1[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder < 10)
        {
            checkDigit = remainder;
        }
        else
        {
            int[] weights2 = { 4, 9, 5, 7 };
            sum = 0;
            for (int i = 0; i < 4; i++)
            {
                sum += (uic[8 + i] - '0') * weights2[i];
            }
            remainder = sum % 11;
            checkDigit = remainder < 10 ? remainder : 0;
        }

        if (checkDigit != (uic[12] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.BulgariaUicInvalidChecksum13);
        }

        return ValidationResult.Success();
    }

    public static string? Format(string? instance)
    {
        return EnterpriseNumberNormalizer.Normalize(instance, "BG");
    }
}
