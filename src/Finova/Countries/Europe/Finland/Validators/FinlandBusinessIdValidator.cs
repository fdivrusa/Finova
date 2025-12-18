using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Finland.Validators;

/// <summary>
/// Validator for Finland Business ID (Y-tunnus).
/// Format: 7 digits + hyphen + 1 check digit (Total 8 numeric digits).
/// </summary>
public partial class FinlandBusinessIdValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "FI";

    public ValidationResult Validate(string? instance) => ValidateBusinessId(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Finland Business ID (Y-tunnus).
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateBusinessId(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.FinlandBusinessIdEmpty);
        }

        // Remove "FI" prefix if present, hyphens, and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("FI"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.FinlandBusinessIdInvalidLength);
        }

        // Weights: [7, 9, 10, 5, 8, 4, 2]
        int[] weights = { 7, 9, 10, 5, 8, 4, 2 };
        int sum = 0;

        for (int i = 0; i < 7; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.FinlandBusinessIdInvalidChecksumRemainder1);
        }
        else
        {
            checkDigit = 11 - remainder;
        }

        int lastDigit = digits[7] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Finland Business ID.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateBusinessId(number).IsValid)
        {
            throw new ArgumentException("Invalid Business ID", nameof(number));
        }

        var normalized = Normalize(number);
        // Format: 1234567-8
        return $"{normalized.Substring(0, 7)}-{normalized.Substring(7, 1)}";
    }

    /// <summary>
    /// Normalizes a Finland Business ID.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("FI"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
