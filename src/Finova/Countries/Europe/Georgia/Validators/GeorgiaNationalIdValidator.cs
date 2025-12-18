using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgia Personal Number (Piradi nomeri).
/// Format: 11 digits.
/// </summary>
public class GeorgiaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "GE";

    /// <summary>
    /// Validates the Georgia Personal Number.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Georgia Personal Number (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        if (sanitized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
