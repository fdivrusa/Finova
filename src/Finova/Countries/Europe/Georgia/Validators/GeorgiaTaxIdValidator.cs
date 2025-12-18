using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgia Tax Identification Number (Saidentifikatsio kodi).
/// Format: 9 digits.
/// </summary>
public partial class GeorgiaTaxIdValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "GE";

    public ValidationResult Validate(string? instance) => ValidateTaxId(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Georgia Tax Identification Number.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateTaxId(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove "GE" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("GE"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidGeorgiaTaxIdLength);
        }

        // Weights: [1, 2, 3, 4, 5, 6, 7, 8]
        int[] weights = [1, 2, 3, 4, 5, 6, 7, 8];
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder < 10)
        {
            checkDigit = remainder;
        }
        else // remainder == 10
        {
            // If Remainder == 10, Number is invalid (or requires fallback, assume Invalid for standard entities).
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidGeorgiaTaxIdChecksumRemainder10);
        }

        int lastDigit = digits[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Georgia Tax ID.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateTaxId(number).IsValid)
        {
            throw new ArgumentException("Invalid Tax ID", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Georgia Tax ID.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("GE"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
