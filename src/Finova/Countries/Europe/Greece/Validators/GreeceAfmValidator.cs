using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greece.Validators;

/// <summary>
/// Validator for Greece AFM (Arithmos Forologikou Mitroou).
/// Format: 9 digits.
/// </summary>
public partial class GreeceAfmValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "GR";

    public ValidationResult Validate(string? instance) => ValidateAfm(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Greece AFM.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateAfm(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove "EL" or "GR" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("EL") || cleaned.StartsWith("GR"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.AfmMustBe9Digits);
        }

        // Weights (Reversed Powers of 2): [256, 128, 64, 32, 16, 8, 4, 2]
        int[] weights = [256, 128, 64, 32, 16, 8, 4, 2];
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder % 10; // If Remainder is 10, CheckDigit is 0.

        int lastDigit = digits[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Greece AFM.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateAfm(number).IsValid)
        {
            throw new ArgumentException("Invalid AFM", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Greece AFM.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("EL") || cleaned.StartsWith("GR"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
