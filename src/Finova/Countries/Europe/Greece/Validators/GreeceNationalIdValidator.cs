using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greece.Validators;

/// <summary>
/// Validates the Greek Social Security Number (AMKA).
/// </summary>
public class GreeceNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "GR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <summary>
    /// Validates the Greek Social Security Number (AMKA) (Static).
    /// </summary>
    /// <param name="input">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string? sanitized = InputSanitizer.Sanitize(input);
        if (string.IsNullOrEmpty(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (sanitized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Luhn Algorithm
        if (!ChecksumHelper.ValidateLuhn(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
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
