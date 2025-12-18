using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Bulgaria.Validators;

/// <summary>
/// Validator for Bulgaria Uniform Civil Number (EGN).
/// </summary>
public class BulgariaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "BG";

    private static readonly int[] Weights = { 2, 4, 8, 5, 10, 9, 7, 3, 6 };

    /// <summary>
    /// Validates the Bulgarian EGN.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Bulgarian EGN (Static).
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

        // Validate Date
        int year = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int day = int.Parse(sanitized.Substring(4, 2));

        int fullYear = 0;
        int realMonth = 0;

        if (month >= 1 && month <= 12)
        {
            fullYear = 1900 + year;
            realMonth = month;
        }
        else if (month >= 21 && month <= 32)
        {
            fullYear = 1800 + year;
            realMonth = month - 20;
        }
        else if (month >= 41 && month <= 52)
        {
            fullYear = 2000 + year;
            realMonth = month - 40;
        }
        else
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        if (!DateHelper.IsValidDate(fullYear, realMonth, day))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Calculate Checksum
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 10 ? 0 : remainder;

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
