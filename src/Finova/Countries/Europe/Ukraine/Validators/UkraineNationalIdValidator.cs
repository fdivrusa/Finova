using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ukraine.Validators;

/// <summary>
/// Validator for Ukraine National Identification Number (RNTRC).
/// Format: 10 digits.
/// </summary>
public class UkraineNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "UA";

    /// <summary>
    /// Validates the Ukraine RNTRC.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Ukraine RNTRC (Static).
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

        if (sanitized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Weights: -1 5 7 9 4 6 10 5 7
        int[] weights = { -1, 5, 7, 9, 4, 6, 10, 5, 7 };
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (remainder == 10) ? 0 : remainder;

        if (checkDigit != (sanitized[9] - '0'))
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
