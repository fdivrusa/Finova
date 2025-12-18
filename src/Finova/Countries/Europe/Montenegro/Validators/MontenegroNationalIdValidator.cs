using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegro National Identification Number (JMBG).
/// Format: DDMMYYYRRBBBK (13 digits).
/// </summary>
public class MontenegroNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "ME";

    /// <summary>
    /// Validates the Montenegro JMBG.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Montenegro JMBG (Static).
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

        // Basic Date Validation (DDMMYYY)
        // Note: YYY is 900-999 for 20th century, 000-onwards for 21st?
        // Actually JMBG format:
        // DD - Day
        // MM - Month
        // YYY - Year (last 3 digits). 9xx -> 19xx, 0xx -> 20xx.
        // RR - Region
        // BBB - Unique number
        // K - Checksum

        // We can implement full date validation if we want, but checksum is usually enough for structure.
        // Let's do checksum.

        // Mod 11
        // Weights: 7 6 5 4 3 2 7 6 5 4 3 2
        int[] weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 12; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1)
        {
            // Remainder 1 is invalid in JMBG
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }
        else
        {
            checkDigit = 11 - remainder;
        }

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
