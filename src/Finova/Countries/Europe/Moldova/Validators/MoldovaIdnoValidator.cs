using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldova IDNO (State Identification Number).
/// Format: 13 digits.
/// </summary>
public partial class MoldovaIdnoValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "MD";

    public ValidationResult Validate(string? instance) => ValidateIdno(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Moldova IDNO.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateIdno(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove spaces
        var cleaned = number.Trim().ToUpperInvariant();
        // No country prefix specified to strip, but let's be safe.
        // Spec: "Sanitization: Remove spaces."

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.IdnoMustBe13Digits);
        }

        // Weights: [7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1] applied to first 12 digits.
        int[] weights = { 7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1 };
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 10;
        int checkDigit = remainder; // Standard ISO 7064 Mod 10? No, spec says "Remainder = Sum % 10".

        int lastDigit = digits[12] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Moldova IDNO.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateIdno(number).IsValid)
        {
            throw new ArgumentException("Invalid IDNO", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Moldova IDNO.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        return DigitsOnlyRegex().Replace(number, "");
    }
}
