using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Gibraltar.Validators;

/// <summary>
/// Validator for Gibraltar Company Registration Number.
/// Format: Usually 5 to 7 digits.
/// </summary>
public partial class GibraltarCompanyNumberValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{5,7}$")]
    private static partial Regex CompanyNumberRegex();

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "GI";

    public ValidationResult Validate(string? instance) => ValidateCompanyNumber(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Gibraltar Company Registration Number.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateCompanyNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove "GI" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("GI"))
        {
            cleaned = cleaned[2..];
        }

        // Strict Regex Validation only: ^\d{5,7}$
        // Checksum is skipped due to lack of public spec.
        if (!CompanyNumberRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidGibraltarCompanyNumberFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Gibraltar Company Number.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateCompanyNumber(number).IsValid)
        {
            throw new ArgumentException("Invalid Company Number", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Gibraltar Company Number.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("GI"))
        {
            cleaned = cleaned[2..];
        }

        // Since we only accept digits in validation, we can just return the cleaned string if it matches regex, 
        // or strip non-digits if we want to be more lenient in normalization (though validation is strict).
        // Let's stick to stripping non-digits for consistency with other validators.
        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
