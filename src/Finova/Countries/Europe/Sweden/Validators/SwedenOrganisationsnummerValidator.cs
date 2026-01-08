using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Sweden.Validators;

/// <summary>
/// Validates Swedish Organisationsnummer (Organization Number).
/// Format: NNNNNN-NNNN (10 digits with optional hyphen after 6th digit)
/// Examples: 556036-0793, 5560360793
/// </summary>
/// <remarks>
/// The Organisationsnummer is assigned by Bolagsverket (Swedish Companies Registration Office)
/// and Skatteverket to all organizations and businesses registered in Sweden.
///
/// This is DISTINCT from the Swedish VAT number (SE + 12 digits ending in 01).
///
/// Format rules:
/// - First 2 digits (GG): Group code (16=individuals, 20+=companies)
///   - 10-12: Estates (dödsbon)
///   - 16-19: Individuals (enskild firma)
///   - 20-23: Limited companies (aktiebolag AB)
///   - 51-54: Partnerships (handelsbolag, kommanditbolag)
///   - 55-59: Cooperative housing, economic associations
///   - 60-69: Non-profit organizations
///   - 70-74: Government institutions
///   - 75-79: Congregations
///   - 80-89: Foreign companies
///   - 90-94: Foundations
///   - 95-99: Other
/// - Middle 4 digits: Sequential number (or birth date for individuals)
/// - Hyphen (optional, placed after 6th digit)
/// - Last 4 digits: Serial number + Luhn check digit
///
/// Check digit: Luhn algorithm on all 10 digits.
/// </remarks>
public partial class SwedenOrganisationsnummerValidator : ITaxIdValidator
{
    private const string CountryPrefix = "SE";

    /// <summary>
    /// Gets the country code for Sweden.
    /// </summary>
    public string CountryCode => CountryPrefix;

    // Pattern: 10 digits with optional hyphen after 6th digit
    private static readonly Regex OrgnummerPattern = OrgnummerRegex();

    [GeneratedRegex(@"^(\d{6})-?(\d{4})$", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex OrgnummerRegex();

    /// <summary>
    /// Validates a Swedish Organisationsnummer.
    /// </summary>
    /// <param name="number">The organization number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? number)
    {
        return ValidateOrganisationsnummer(number);
    }

    /// <summary>
    /// Validates a Swedish Organisationsnummer (static method).
    /// </summary>
    /// <param name="number">The organization number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateOrganisationsnummer(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string normalized = Normalize(number);
        if (string.IsNullOrEmpty(normalized) || normalized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwedenOrganisationsnummerFormat);
        }

        // Check that all characters are digits
        if (!normalized.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwedenOrganisationsnummerFormat);
        }

        // Validate the first two digits (group code) - must be >= 10
        int groupCode = int.Parse(normalized[..2]);
        if (groupCode < 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwedenOrganisationsnummerFormat);
        }

        // For companies (not individuals), the 3rd digit must be >= 2
        // Individual entrepreneurs (enskild firma) have group codes 16-19
        if (groupCode < 16 || groupCode > 19)
        {
            // For non-individual entities, third digit should normally be >= 2
            // This helps distinguish from personal identity numbers
            int thirdDigit = normalized[2] - '0';
            if (thirdDigit < 2)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwedenOrganisationsnummerFormat);
            }
        }

        // Validate Luhn check digit
        if (!IsValidLuhnCheckDigit(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidSwedenOrganisationsnummerCheckDigit);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the Luhn check digit.
    /// </summary>
    private static bool IsValidLuhnCheckDigit(string number)
    {
        int sum = 0;
        bool alternate = false;

        // Process from right to left
        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = number[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    /// <summary>
    /// Parses and normalizes a Swedish Organisationsnummer.
    /// </summary>
    /// <param name="number">The organization number to normalize.</param>
    /// <returns>The normalized organization number or null if invalid.</returns>
    public string? Parse(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        string normalized = Normalize(number);
        return ValidateOrganisationsnummer(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Normalizes a Swedish Organisationsnummer by removing hyphens and whitespace.
    /// Returns format: 10 digits without hyphen.
    /// </summary>
    /// <param name="number">The organization number to normalize.</param>
    /// <returns>The normalized organization number.</returns>
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

        // Remove optional SE prefix (sometimes used)
        if (cleaned.StartsWith("SE", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        return cleaned;
    }

    /// <summary>
    /// Formats a Swedish Organisationsnummer with hyphen (NNNNNN-NNNN).
    /// </summary>
    /// <param name="number">The organization number to format.</param>
    /// <returns>The formatted organization number or null if invalid.</returns>
    public static string? Format(string? number)
    {
        string normalized = Normalize(number);
        if (string.IsNullOrEmpty(normalized) || normalized.Length != 10)
        {
            return null;
        }

        return $"{normalized[..6]}-{normalized[6..]}";
    }
}
