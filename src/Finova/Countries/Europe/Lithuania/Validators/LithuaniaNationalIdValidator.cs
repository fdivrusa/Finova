using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Lithuania.Validators;

/// <summary>
/// Validator for Lithuania Personal Code (Asmens kodas).
/// </summary>
public class LithuaniaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "LT";

    private static readonly int[] Weights1 = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 1 };
    private static readonly int[] Weights2 = { 3, 4, 5, 6, 7, 8, 9, 1, 2, 3 };

    /// <summary>
    /// Validates the Lithuanian Asmens kodas.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Lithuanian Asmens kodas (Static).
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

        // Validate Gender/Century (1st digit)
        int genderCentury = sanitized[0] - '0';
        if (genderCentury < 1 || genderCentury > 6)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid gender/century digit.");
        }

        // Validate Date
        int year = (sanitized[1] - '0') * 10 + (sanitized[2] - '0');
        int month = (sanitized[3] - '0') * 10 + (sanitized[4] - '0');
        int day = (sanitized[5] - '0') * 10 + (sanitized[6] - '0');

        // Determine full year
        int fullYear = 0;
        if (genderCentury == 1 || genderCentury == 2) fullYear = 1800 + year;
        else if (genderCentury == 3 || genderCentury == 4) fullYear = 1900 + year;
        else if (genderCentury == 5 || genderCentury == 6) fullYear = 2000 + year;

        if (!DateHelper.IsValidDate(fullYear, month, day))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Calculate Checksum
        int remainder = ChecksumHelper.CalculateWeightedModulo11(sanitized.Substring(0, 10), Weights1);
        int checkDigit;

        if (remainder == 10)
        {
            remainder = ChecksumHelper.CalculateWeightedModulo11(sanitized.Substring(0, 10), Weights2);
            if (remainder == 10)
            {
                checkDigit = 0;
            }
            else
            {
                checkDigit = remainder;
            }
        }
        else
        {
            checkDigit = remainder;
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
