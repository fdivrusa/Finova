using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Slovenia.Validators;

/// <summary>
/// Validates Slovenian Matična številka (Registration Number / Business ID).
/// Format: 10 digits with modulo 11 check digit
/// Example: 5000001000
/// </summary>
/// <remarks>
/// The Matična številka is assigned by AJPES (Agency of the Republic of Slovenia for Public Legal Records
/// and Related Services) to all business entities registered in Slovenia.
///
/// This is DISTINCT from the Slovenian VAT number (SI + 8 digits, also called Davčna številka).
///
/// Format breakdown:
/// - First 7 digits: base registration number
/// - 8th digit: check digit (modulo 11)
/// - Last 2 digits: subdivision code (00-99)
/// </remarks>
public partial class SloveniaMaticnaStevilkaValidator : ITaxIdValidator
{
    private const string CountryPrefix = "SI";

    /// <summary>
    /// Gets the country code for Slovenia.
    /// </summary>
    public string CountryCode => CountryPrefix;

    private static readonly Regex MaticnaStevilkaPattern = MaticnaStevilkaRegex();

    [GeneratedRegex(@"^\d{10}$", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex MaticnaStevilkaRegex();

    /// <summary>
    /// Validates a Slovenian Matična številka.
    /// </summary>
    /// <param name="number">The registration number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? number)
    {
        return ValidateMaticnaStevilka(number);
    }

    /// <summary>
    /// Validates a Slovenian Matična številka (static method).
    /// </summary>
    /// <param name="number">The registration number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateMaticnaStevilka(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string normalized = Normalize(number);
        if (string.IsNullOrEmpty(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSloveniaMaticnaStevilkaFormat);
        }

        // Must be exactly 10 digits
        if (!MaticnaStevilkaPattern.IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSloveniaMaticnaStevilkaFormat);
        }

        // Cannot start with 0
        if (normalized[0] == '0')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSloveniaMaticnaStevilkaFormat);
        }

        // Validate check digit (8th digit) using modulo 11 algorithm
        if (!IsValidCheckDigit(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidSloveniaMaticnaStevilkaCheckDigit);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the check digit using the modulo 11 algorithm.
    /// </summary>
    private static bool IsValidCheckDigit(string maticna)
    {
        // Extract first 7 digits for check calculation
        // Weights for positions 1-7
        int[] weights = [7, 6, 5, 4, 3, 2, 1];

        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (maticna[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int expectedCheckDigit = (11 - remainder) % 11;

        // If check digit would be 10, it's invalid (single digit required)
        if (expectedCheckDigit == 10)
        {
            return false;
        }

        int actualCheckDigit = maticna[7] - '0';
        return actualCheckDigit == expectedCheckDigit;
    }

    /// <summary>
    /// Parses and normalizes a Slovenian Matična številka.
    /// </summary>
    /// <param name="number">The registration number to normalize.</param>
    /// <returns>The normalized registration number or null if invalid.</returns>
    public string? Parse(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        string normalized = Normalize(number);
        return ValidateMaticnaStevilka(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Normalizes a Slovenian Matična številka.
    /// </summary>
    /// <param name="number">The registration number to normalize.</param>
    /// <returns>The normalized registration number.</returns>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        // Remove spaces, dashes
        string cleaned = number.Trim()
                               .Replace(" ", "")
                               .Replace("-", "")
                               .Replace(".", "");

        // Remove optional SI prefix
        if (cleaned.StartsWith("SI", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        return cleaned;
    }
}
