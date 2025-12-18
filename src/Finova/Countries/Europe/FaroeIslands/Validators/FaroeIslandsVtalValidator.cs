using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands V-tal (Virdiskanummur).
/// Format: 6 digits.
/// </summary>
public partial class FaroeIslandsVtalValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "FO";

    public ValidationResult Validate(string? instance) => ValidateVtal(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Faroe Islands V-tal.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateVtal(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove "FO" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("FO"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 6)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidVtalLength);
        }

        // Weights: [2, 7, 6, 5, 4, 3]
        int[] weights = { 2, 7, 6, 5, 4, 3 };
        int sum = 0;

        for (int i = 0; i < 6; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        if (sum % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Faroe Islands V-tal.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateVtal(number).IsValid)
        {
            throw new ArgumentException("Invalid V-tal", nameof(number));
        }

        var normalized = Normalize(number);
        return normalized; // Usually just 6 digits
    }

    /// <summary>
    /// Normalizes a Faroe Islands V-tal.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("FO"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
