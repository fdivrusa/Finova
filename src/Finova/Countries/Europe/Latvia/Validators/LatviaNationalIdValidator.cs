using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Latvia.Validators;

/// <summary>
/// Validator for Latvia Personal Code (Personas kods).
/// </summary>
public class LatviaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "LV";

    private static readonly int[] Weights = { 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };

    /// <summary>
    /// Validates the Latvian Personas kods.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Latvian Personas kods (Static).
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

        // Check for new format (starting with 32)
        if (sanitized.StartsWith("32"))
        {
            // New format: 32xxxxxxxxx. No checksum, no date info.
            // Just 11 digits starting with 32.
            return ValidationResult.Success();
        }

        // Old format: DDMMYYXXXXX
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int year = int.Parse(sanitized.Substring(4, 2));

        // Century adjustment?
        // Usually 1800-2099.
        // 7th digit (century):
        // 0 -> 1800
        // 1 -> 1900
        // 2 -> 2000
        int centuryDigit = sanitized[6] - '0';
        int fullYear = 0;
        if (centuryDigit == 0) fullYear = 1800 + year;
        else if (centuryDigit == 1) fullYear = 1900 + year;
        else if (centuryDigit == 2) fullYear = 2000 + year;
        else
        {
            // If century digit is not 0,1,2, it might be invalid or different logic.
            // But let's assume standard.
        }

        if (fullYear != 0)
        {
            if (!DateHelper.IsValidDate(fullYear, month, day))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }
        }
        else
        {
            // Fallback date check without year
             if (month < 1 || month > 12 || day < 1 || day > 31)
             {
                 return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
             }
        }

        // Checksum for old format
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (1 - remainder + 11) % 11;

        if (checkDigit == 10)
        {
            // Invalid checksum calculation result (cannot be 10)
            // This implies the number is invalid if the formula yields 10.
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        if (checkDigit != (sanitized[10] - '0'))
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
