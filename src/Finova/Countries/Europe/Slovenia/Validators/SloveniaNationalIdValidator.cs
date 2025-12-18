using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Slovenia.Validators;

/// <summary>
/// Validator for Slovenia Unique Master Citizen Number (EMŠO).
/// </summary>
public class SloveniaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "SI";

    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <summary>
    /// Validates the Slovenian EMŠO.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Slovenian EMŠO (Static).
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

        // Validate Date (DDMMYYY)
        // YYY is 3 digits. e.g. 980 -> 1980. 000 -> 2000.
        // 800-999 -> 1xxx.
        // 000-600 -> 2xxx.
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int yearPart = int.Parse(sanitized.Substring(4, 3));

        int fullYear = 0;
        if (yearPart >= 800) fullYear = 1000 + yearPart; // 1800-1999? No, usually 9xx is 19xx.
        // Wait, EMŠO introduced in 1976.
        // 9xx -> 19xx.
        // 0xx -> 20xx.
        // 8xx -> 18xx?
        // Let's assume:
        if (yearPart >= 850) fullYear = 1000 + yearPart; // 1850-1999
        else fullYear = 2000 + yearPart; // 2000-2849

        if (!DateHelper.IsValidDate(fullYear, month, day))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Calculate Checksum
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1)
        {
            // Remainder 1 implies check digit would be 10, which is invalid.
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
