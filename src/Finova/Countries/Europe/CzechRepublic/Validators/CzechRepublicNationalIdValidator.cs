using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

/// <summary>
/// Validates the Czech Birth Number (Rodné číslo).
/// </summary>
public class CzechRepublicNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CZ";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <summary>
    /// Validates the Czech Birth Number (Rodné číslo) (Static).
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

        // Length: 9 or 10 digits
        if (sanitized.Length != 9 && sanitized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out long number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Before 1954: 9 digits, no checksum
        // After 1954: 10 digits, Mod 11
        if (sanitized.Length == 9)
        {
            // Basic date check could be added here
            return ValidationResult.Success();
        }

        // 10 digits: Mod 11
        // The whole number must be divisible by 11
        // Exception: if remainder is 10, check digit is 0 (handled by divisible check usually? No, specific rule)
        // Actually: "If the remainder is 10, the check digit is 0." -> This implies the number ends in 0 and sum%11==10?
        // Standard rule: The entire 10-digit number must be divisible by 11.
        // Exception: If remainder is 10, the check digit is 0.
        // Wait, if number % 11 == 0, it's valid.
        
        if (number % 11 != 0)
        {
            // Special case: remainder 10 -> check digit 0
            // But if check digit is 0, and remainder was 10, then number % 11 would not be 0.
            // Let's check the first 9 digits.
            long first9 = long.Parse(sanitized.Substring(0, 9));
            long remainder = first9 % 11;
            int checkDigit = sanitized[9] - '0';

            if (remainder == 10)
            {
                if (checkDigit != 0)
                {
                    return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
                }
            }
            else
            {
                if (checkDigit != remainder)
                {
                    return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
                }
            }
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
