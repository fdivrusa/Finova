using System.Text.RegularExpressions;
using Finova.Core.Common;

namespace Finova.Countries.Asia.Singapore.Validators;

/// <summary>
/// Validates Singapore Unique Entity Number (UEN).
/// </summary>
public static partial class SingaporeUenValidator
{
    // Business: 8 digits + 1 letter (e.g., 12345678A)
    [GeneratedRegex(@"^\d{8}[A-Z]$")]
    private static partial Regex BusinessUenRegex();

    // Local Company: YYYY + 5 digits + 1 letter (e.g., 202312345A)
    [GeneratedRegex(@"^\d{9}[A-Z]$")]
    private static partial Regex LocalCompanyUenRegex();

    // Other Entities: T/S/R + 2 digits + 2 letters + 4 digits + 1 letter (e.g., T08LL1234A)
    [GeneratedRegex(@"^[TSR]\d{2}[A-Z]{2}\d{4}[A-Z]$")]
    private static partial Regex OtherEntityUenRegex();

    /// <summary>
    /// Validates a Singapore UEN.
    /// </summary>
    /// <param name="uen">The UEN string.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? uen)
    {
        if (string.IsNullOrWhiteSpace(uen))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = uen.Trim().ToUpperInvariant();

        if (clean.Length != 9 && clean.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSingaporeUenLength);
        }

        bool isValidFormat = BusinessUenRegex().IsMatch(clean) ||
                             LocalCompanyUenRegex().IsMatch(clean) ||
                             OtherEntityUenRegex().IsMatch(clean);

        if (!isValidFormat)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSingaporeUenFormat);
        }

        // Checksum validation is complex and varies by entity type.
        // For offline validation without a comprehensive algorithm implementation for all types,
        // we rely on strict format validation.

        return ValidationResult.Success();
    }
}
