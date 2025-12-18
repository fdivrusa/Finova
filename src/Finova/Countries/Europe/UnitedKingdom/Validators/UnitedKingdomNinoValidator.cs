using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom National Insurance Number (NINO).
/// </summary>
public partial class UnitedKingdomNinoValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "GB";

    /// <summary>
    /// Validates the UK National Insurance Number.
    /// </summary>
    /// <param name="nationalId">The NINO to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the UK National Insurance Number (Static).
    /// </summary>
    /// <param name="nationalId">The NINO to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Normalize: Remove spaces, uppercase
        string normalized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        if (normalized.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Regex check
        if (!NinoRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Specific invalid prefixes
        string prefix = normalized.Substring(0, 2);
        if (IsInvalidPrefix(prefix))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid NINO prefix.");
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }

    private static bool IsInvalidPrefix(string prefix)
    {
        return prefix == "GB" || prefix == "BG" || prefix == "NK" || prefix == "TN" || prefix == "ZZ";
    }

    [GeneratedRegex("^[A-CEGHJ-PR-TW-Z]{1}[A-CEGHJ-NPR-TW-Z]{1}[0-9]{6}[A-D]{1}$")]
    private static partial Regex NinoRegex();
}
