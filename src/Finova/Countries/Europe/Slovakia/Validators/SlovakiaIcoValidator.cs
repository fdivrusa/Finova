using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Slovakia.Validators;

/// <summary>
/// Validates Slovak IČO (Identifikačné číslo organizácie) - Business Identification Number.
/// Format: 8 digits with modulo 11 check digit
/// Example: 12345678
/// </summary>
/// <remarks>
/// The IČO is assigned by the Statistical Office of the Slovak Republic to all legal entities
/// and entrepreneurs registered in Slovakia. It uniquely identifies a business entity.
///
/// This is DISTINCT from the Slovak VAT number (SK + 10 digits with IČ DPH format).
///
/// Check digit calculation: Modulo 11 algorithm using weights 8,7,6,5,4,3,2 for first 7 digits.
/// </remarks>
public partial class SlovakiaIcoValidator : ITaxIdValidator
{
    private const string CountryPrefix = "SK";

    /// <summary>
    /// Gets the country code for Slovakia.
    /// </summary>
    public string CountryCode => CountryPrefix;

    private static readonly Regex IcoPattern = IcoRegex();

    [GeneratedRegex(@"^\d{8}$", RegexOptions.None, matchTimeoutMilliseconds: 1000)]
    private static partial Regex IcoRegex();

    /// <summary>
    /// Validates a Slovak IČO number.
    /// </summary>
    /// <param name="number">The IČO to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? number)
    {
        return ValidateIco(number);
    }

    /// <summary>
    /// Validates a Slovak IČO number (static method).
    /// </summary>
    /// <param name="number">The IČO to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateIco(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string normalized = Normalize(number);
        if (string.IsNullOrEmpty(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakiaIcoFormat);
        }

        // Must be exactly 8 digits
        if (!IcoPattern.IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakiaIcoFormat);
        }

        // Cannot start with 0
        if (normalized[0] == '0')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovakiaIcoFormat);
        }

        // Validate check digit using modulo 11 algorithm
        if (!IsValidCheckDigit(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidSlovakiaIcoCheckDigit);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the check digit using the modulo 11 algorithm.
    /// </summary>
    private static bool IsValidCheckDigit(string ico)
    {
        // Weights for positions 1-7
        int[] weights = [8, 7, 6, 5, 4, 3, 2];

        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (ico[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int expectedCheckDigit = remainder switch
        {
            0 => 1,
            1 => 0,
            _ => 11 - remainder
        };

        int actualCheckDigit = ico[7] - '0';
        return actualCheckDigit == expectedCheckDigit;
    }

    /// <summary>
    /// Parses and normalizes a Slovak IČO number.
    /// </summary>
    /// <param name="number">The IČO to normalize.</param>
    /// <returns>The normalized IČO or null if invalid.</returns>
    public string? Parse(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        string normalized = Normalize(number);
        return ValidateIco(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Normalizes a Slovak IČO number by removing whitespace and formatting characters.
    /// </summary>
    /// <param name="number">The IČO to normalize.</param>
    /// <returns>The normalized IČO.</returns>
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

        // Remove optional SK prefix
        if (cleaned.StartsWith("SK", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        return cleaned;
    }
}
