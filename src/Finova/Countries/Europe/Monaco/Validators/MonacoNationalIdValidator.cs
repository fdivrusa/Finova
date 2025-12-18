using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Monaco.Validators;

/// <summary>
/// Validator for Monaco National Identification Number.
/// Monaco uses a Carte d'Identité.
/// Format: 4 or 5 digits usually.
/// </summary>
public class MonacoNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "MC";

    /// <summary>
    /// Validates the Monaco National ID.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Monaco National ID (Static).
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

        // Length check: 4 to 10 digits to be safe, usually short.
        if (sanitized.Length < 4 || sanitized.Length > 10)
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
