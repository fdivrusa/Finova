using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norway National Identity Number (Fødselsnummer).
/// Format: DDMMYYXXXXX (11 digits).
/// </summary>
public class NorwayNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "NO";

    /// <summary>
    /// Validates the Norway Fødselsnummer.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Norway Fødselsnummer (Static).
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

        if (sanitized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Checksum 1 (d10)
        // Weights: 3 7 6 1 8 9 4 5 2
        int[] weights1 = { 3, 7, 6, 1, 8, 9, 4, 5, 2 };
        int sum1 = 0;
        for (int i = 0; i < 9; i++)
        {
            sum1 += (sanitized[i] - '0') * weights1[i];
        }
        int remainder1 = sum1 % 11;
        int checkDigit1 = (remainder1 == 0) ? 0 : 11 - remainder1;
        if (checkDigit1 == 10) return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);

        if (checkDigit1 != (sanitized[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // Checksum 2 (d11)
        // Weights: 5 4 3 2 7 6 5 4 3 2
        int[] weights2 = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum2 = 0;
        for (int i = 0; i < 10; i++)
        {
            sum2 += (sanitized[i] - '0') * weights2[i];
        }
        int remainder2 = sum2 % 11;
        int checkDigit2 = (remainder2 == 0) ? 0 : 11 - remainder2;
        if (checkDigit2 == 10) return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);

        if (checkDigit2 != (sanitized[10] - '0'))
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
