using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validates the Swiss Social Security Number (AHV/AVS).
/// </summary>
public class SwitzerlandNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CH";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <summary>
    /// Validates the Swiss Social Security Number (AHV/AVS) (Static).
    /// </summary>
    /// <param name="input">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove dots
        string? sanitized = InputSanitizer.Sanitize(input);
        if (string.IsNullOrEmpty(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (sanitized.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!sanitized.StartsWith("756"))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Must start with 756.");
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // EAN-13 Checksum (Mod 10)
        // Weights: 1, 3, 1, 3...
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            int digit = sanitized[i] - '0';
            sum += (i % 2 == 0) ? digit : digit * 3;
        }

        int remainder = sum % 10;
        int checkDigit = remainder == 0 ? 0 : 10 - remainder;

        if (checkDigit != (sanitized[12] - '0'))
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
