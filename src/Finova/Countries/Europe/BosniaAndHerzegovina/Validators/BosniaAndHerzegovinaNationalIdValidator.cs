using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

/// <summary>
/// Validator for Bosnia and Herzegovina Unique Master Citizen Number (JMBG).
/// </summary>
public class BosniaAndHerzegovinaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "BA";

    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <summary>
    /// Validates the Bosnian JMBG.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Bosnian JMBG (Static).
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
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int yearPart = int.Parse(sanitized.Substring(4, 3));

        int fullYear;
        if (yearPart >= 800) fullYear = 1000 + yearPart; // 1800-1999
        else fullYear = 2000 + yearPart; // 2000-2xxx

        if (!DateHelper.IsValidDate(fullYear, month, day))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Region (RR) - Digits 8 and 9
        // Bosnia regions are 10-19
        int region = int.Parse(sanitized.Substring(7, 2));
        if (region < 10 || region > 19)
        {
            // Note: Some sources say strict validation might reject valid IDs if people moved or were born elsewhere.
            // However, for a "Bosnia National ID" validator, we typically expect the Bosnia region code.
            // If we want to be lenient, we could remove this check or make it optional.
            // For now, I'll comment it out or leave it as a warning? 
            // No, validation should be strict based on the definition.
            // But JMBG is valid regardless of region.
            // I will NOT enforce region 10-19 strictly because a Bosnian citizen could have been born in another republic of Yugoslavia before the breakup.
            // But wait, if they were born in Serbia, they have a Serbian JMBG. Does that count as a Bosnian National ID?
            // Yes, if they are a citizen. The JMBG doesn't change.
            // So I should NOT enforce region 10-19.
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
