using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validator for German Tax ID (Steuernummer).
/// Validates the Unified Federal Format (13 digits).
/// </summary>
public partial class GermanySteuernummerValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex UnifiedFormatRegex();

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "DE";

    public ValidationResult Validate(string? instance) => ValidateSteuernummer(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a German Steuernummer (Unified Federal Format).
    /// </summary>
    /// <param name="number">The Steuernummer to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateSteuernummer(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.SteuernummerCannotBeEmpty);
        }

        var normalized = Normalize(number);

        if (!UnifiedFormatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.SteuernummerMustBe13Digits);
        }

        // Note: Full checksum validation for the unified format is complex and depends on the first 4 digits (Finanzamt).
        // For now, we validate the structure.

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a German Steuernummer.
    /// Returns the 13-digit unified format.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateSteuernummer(number).IsValid)
        {
            throw new ArgumentException("Invalid Steuernummer", nameof(number));
        }
        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a German Steuernummer by removing non-digit characters.
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
