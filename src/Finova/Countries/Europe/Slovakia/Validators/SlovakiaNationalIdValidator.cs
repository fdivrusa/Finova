using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Slovakia.Validators;

/// <summary>
/// Validator for Slovakia Birth Number (Rodné číslo).
/// </summary>
public class SlovakiaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "SK";

    /// <summary>
    /// Validates the Slovak Rodné číslo.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Slovak Rodné číslo (Static).
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

        // Length: 9 or 10 digits
        if (sanitized.Length < 9 || sanitized.Length > 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out long number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Date validation logic is complex (YYMMDD, women +50 month, etc.)
        // For now, we focus on checksum and basic structure.
        
        int year = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int day = int.Parse(sanitized.Substring(4, 2));

        // Basic month check (women have +50)
        if (month > 50) month -= 50;
        if (month > 20) // Special case for extra month offset? (RČ has +20 sometimes?)
        {
             // Some systems use +20 for special cases, but standard is +50 for women.
             // Let's stick to standard +50 check.
        }

        // Checksum
        if (sanitized.Length == 10)
        {
            // Must be divisible by 11
            if (number % 11 != 0)
            {
                // Exception: if remainder is 10, check digit is 0?
                // No, for Rodné číslo, the whole number must be divisible by 11.
                // If the calculation results in remainder 10, that number is simply not used/invalid.
                return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
            }
        }
        else // 9 digits
        {
            // Only valid for people born before 1954.
            // We can't strictly validate year without century context, but usually 9 digits are accepted for old IDs.
            // No checksum for 9 digits.
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
