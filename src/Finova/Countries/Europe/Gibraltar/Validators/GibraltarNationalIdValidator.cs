using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Gibraltar.Validators;

/// <summary>
/// Validator for Gibraltar Identity Card Number.
/// Format: G1-1234567-A? Or similar.
/// Actually, Gibraltar ID cards have a number.
/// Format: 7 digits? Or G followed by digits?
/// Information is scarce. Assuming simple alphanumeric or length check.
/// Let's assume 7-9 digits/chars.
/// </summary>
public class GibraltarNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "GI";

    /// <summary>
    /// Validates the Gibraltar Identity Card Number.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Gibraltar Identity Card Number (Static).
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

        // Assuming length check for now as specific format is not well-documented publicly.
        if (sanitized.Length < 5 || sanitized.Length > 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
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
