using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein PEID (Public Enterprise ID) / UID.
/// Format: CHE-123.456.789 or similar, but sanitized for CHE or LI prefix.
/// Logic: Same as Swiss UID.
/// </summary>
public partial class LiechtensteinPeidValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "LI";

    public ValidationResult Validate(string? instance) => ValidatePeid(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Liechtenstein PEID.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidatePeid(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove "CHE" or "LI" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("CHE") || cleaned.StartsWith("LI"))
        {
            // CHE is 3 chars, LI is 2 chars.
            if (cleaned.StartsWith("CHE")) cleaned = cleaned[3..];
            else cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLiechtensteinPeidLength);
        }

        // Swiss UID Logic: Modulo 11 with weights 5,4,3,2,7,6,5,4
        int[] weights = { 5, 4, 3, 2, 7, 6, 5, 4 };
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (11 - remainder) % 11;

        if (checkDigit == 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidLiechtensteinPeidChecksumCheckDigit10);
        }

        int lastDigit = digits[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Liechtenstein PEID.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidatePeid(number).IsValid)
        {
            throw new ArgumentException("Invalid PEID", nameof(number));
        }

        var normalized = Normalize(number);
        // Format: CHE-123.456.789 (Using CHE as it's standard for UID, or maybe LI?)
        // User said: "Format: UID format (CHE-123.456.789)".
        // But context says "LI entities use Swiss UID for VAT, but have a local 'FL-Nummer'".
        // The validator is for PEID/UID. Let's stick to CHE prefix for formatting if it's UID.
        return $"CHE-{normalized.Substring(0, 3)}.{normalized.Substring(3, 3)}.{normalized.Substring(6, 3)}";
    }

    /// <summary>
    /// Normalizes a Liechtenstein PEID.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("CHE") || cleaned.StartsWith("LI"))
        {
            if (cleaned.StartsWith("CHE")) cleaned = cleaned[3..];
            else cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
