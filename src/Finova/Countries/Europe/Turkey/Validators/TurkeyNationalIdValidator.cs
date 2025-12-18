using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Turkey.Validators;

/// <summary>
/// Validator for Turkey National Identification Number (T.C. Kimlik No).
/// Format: 11 digits.
/// </summary>
public class TurkeyNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "TR";

    /// <summary>
    /// Validates the Turkey T.C. Kimlik No.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Turkey T.C. Kimlik No (Static).
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

        if (sanitized[0] == '0')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        int[] digits = new int[11];
        for (int i = 0; i < 11; i++)
        {
            digits[i] = sanitized[i] - '0';
        }

        // Algorithm:
        // d10 = ((d1+d3+d5+d7+d9)*7 - (d2+d4+d6+d8)) % 10
        int sumOdd = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
        int sumEven = digits[1] + digits[3] + digits[5] + digits[7];
        
        int d10 = ((sumOdd * 7) - sumEven) % 10;
        if (d10 < 0) d10 += 10;

        if (digits[9] != d10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // d11 = (d1+d2+d3+d4+d5+d6+d7+d8+d9+d10) % 10
        int sumAll = 0;
        for (int i = 0; i < 10; i++)
        {
            sumAll += digits[i];
        }

        int d11 = sumAll % 10;

        if (digits[10] != d11)
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
