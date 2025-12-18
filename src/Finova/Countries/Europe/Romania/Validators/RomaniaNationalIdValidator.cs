using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Romania.Validators;

/// <summary>
/// Validates the Romanian Personal Numeric Code (CNP).
/// </summary>
public class RomaniaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "RO";

    private static readonly int[] Weights = { 2, 7, 9, 1, 4, 6, 3, 5, 8, 2, 7, 9 };

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <summary>
    /// Validates the Romanian Personal Numeric Code (CNP) (Static).
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

        if (sanitized.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Calculate Checksum
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 10 ? 1 : remainder;

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
