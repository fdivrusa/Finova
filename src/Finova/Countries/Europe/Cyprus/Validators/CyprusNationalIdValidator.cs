using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Cyprus.Validators;

/// <summary>
/// Validator for Cyprus National Identity Card Number.
/// </summary>
public class CyprusNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CY";

    /// <summary>
    /// Validates the Cyprus National ID.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Cyprus National ID (Static).
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

        // Cyprus ID numbers are typically 6 to 8 digits.
        // Allowing 6-10 to be safe.
        if (sanitized.Length < 6 || sanitized.Length > 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // No known public checksum algorithm for the ID number itself.
        
        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
