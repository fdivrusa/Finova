using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Austria.Validators;

/// <summary>
/// Validates the Austrian Social Security Number (Versicherungsnummer).
/// </summary>
public class AustriaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AT";

    private static readonly int[] Weights = { 3, 7, 9, 0, 5, 8, 4, 2, 1, 6 };

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <summary>
    /// Validates the Austrian Social Security Number (Versicherungsnummer) (Static).
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

        if (sanitized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Calculate Checksum
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        if (remainder == 10)
        {
            // Remainder 10 is invalid for SV-Nummer
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        int checkDigit = sanitized[3] - '0';
        if (checkDigit != remainder)
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
