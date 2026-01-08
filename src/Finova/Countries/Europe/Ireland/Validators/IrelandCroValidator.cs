using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validates Irish Company Registration Office (CRO) numbers.
/// Format: 6 digits, or optional prefix (letter) + up to 6 digits
/// Examples: 123456, A12345, IE123456
/// </summary>
/// <remarks>
/// The CRO number (also known as Company Number) is assigned by the Companies Registration Office
/// to all companies registered in Ireland. It uniquely identifies a company in the CRO database.
///
/// This is DISTINCT from the Irish VAT number (IE + 8 or 9 characters with check digit).
/// </remarks>
public partial class IrelandCroValidator : ITaxIdValidator
{
    private const string CountryPrefix = "IE";

    /// <summary>
    /// Gets the country code for Ireland.
    /// </summary>
    public string CountryCode => CountryPrefix;

    // Basic format: 6 digits, or optional letter prefix + up to 6 digits
    // The CRO database uses numbers like: 123456, A12345, IE123456
    private static readonly Regex CroPattern = CroRegex();

    [GeneratedRegex(@"^(?:IE)?([A-Z]?\d{1,6})$", RegexOptions.IgnoreCase, matchTimeoutMilliseconds: 1000)]
    private static partial Regex CroRegex();

    /// <summary>
    /// Validates an Irish CRO number.
    /// </summary>
    /// <param name="number">The CRO number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? number)
    {
        return ValidateCro(number);
    }

    /// <summary>
    /// Validates an Irish CRO number (static method).
    /// </summary>
    /// <param name="number">The CRO number to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateCro(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string normalized = Normalize(number);
        if (string.IsNullOrEmpty(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIrelandCroFormat);
        }

        // After normalization, should be letter(optional) + 1-6 digits
        if (!CroPattern.IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIrelandCroFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Parses and normalizes an Irish CRO number.
    /// </summary>
    /// <param name="number">The CRO number to normalize.</param>
    /// <returns>The normalized CRO number or null if invalid.</returns>
    public string? Parse(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return null;
        }

        string normalized = Normalize(number);
        return ValidateCro(normalized).IsValid ? normalized : null;
    }

    /// <summary>
    /// Normalizes an Irish CRO number by removing whitespace, dashes, and optional IE prefix.
    /// </summary>
    /// <param name="number">The CRO number to normalize.</param>
    /// <returns>The normalized CRO number.</returns>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        // Remove spaces, dashes, dots
        string cleaned = number.Trim()
                               .Replace(" ", "")
                               .Replace("-", "")
                               .Replace(".", "")
                               .ToUpperInvariant();

        // Remove optional IE prefix
        if (cleaned.StartsWith("IE", StringComparison.OrdinalIgnoreCase))
        {
            cleaned = cleaned[2..];
        }

        return cleaned;
    }
}
