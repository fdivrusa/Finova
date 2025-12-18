using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldova National Identification Number (IDNP).
/// Format: 13 digits.
/// </summary>
public class MoldovaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "MD";

    /// <summary>
    /// Validates the Moldova IDNP.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Moldova IDNP (Static).
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

        if (sanitized.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Checksum Algorithm: Mod 10 with weights 7, 3, 1...
        int[] weights = { 7, 3, 1, 7, 3, 1, 7, 3, 1, 7, 3, 1 };
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        int remainder = sum % 10;
        int checkDigit = remainder;

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
