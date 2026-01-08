using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Malta.Validators;

/// <summary>
/// Validates Maltese Company Registration Numbers.
/// Format: C followed by 5-6 digits
/// Examples: C12345, C123456
/// </summary>
/// <remarks>
/// The Maltese Company Number is assigned by the Malta Business Registry (MBR).
/// Companies are prefixed with 'C', partnerships with 'P', foundations with 'F', etc.
///
/// This is DISTINCT from the Maltese VAT number (MT + 8 digits).
/// </remarks>
public partial class MaltaCompanyNumberValidator : ITaxIdValidator
{
    private const string CountryPrefix = "MT";

    /// <summary>
    /// Gets the country code for Malta.
    /// </summary>
    public string CountryCode => CountryPrefix;

    // Format: C + 5-6 digits (most common for limited liability companies)
    // Other prefixes exist: P (partnerships), F (foundations), etc.
    private static readonly Regex CompanyNumberPattern = CompanyNumberRegex();

    [GeneratedRegex(@"^([CPFOE])(\d{5,6})$", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 1000)]
    private static partial Regex CompanyNumberRegex();

    /// <summary>
    /// Validates a Maltese company registration number.
    /// </summary>
    /// <param name="number">The company number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? number)
    {
        return ValidateCompanyNumber(number);
    }

    /// <summary>
    /// Validates a Maltese company registration number (static method).
    /// </summary>
    /// <param name="number">The company number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateCompanyNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string normalized = Normalize(number);
        if (string.IsNullOrEmpty(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMaltaCompanyNumberFormat);
        }

        // Must match pattern: prefix letter + 5-6 digits
        if (!CompanyNumberPattern.IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMaltaCompanyNumberFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses and normalizes a Maltese company registration number.
    /// </summary>
    /// <param name="number">The company number to normalize.</param>
    /// <returns>The normalized company number or null if invalid.</returns>
    public string? Parse(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        string normalized = Normalize(number);
        return ValidateCompanyNumber(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Normalizes a Maltese company registration number.
    /// </summary>
    /// <param name="number">The company number to normalize.</param>
    /// <returns>The normalized company number.</returns>
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
                               .Replace(".", "")
                               .ToUpperInvariant();

        // Remove optional MT prefix
        if (cleaned.StartsWith("MT", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        return cleaned;
    }
}
