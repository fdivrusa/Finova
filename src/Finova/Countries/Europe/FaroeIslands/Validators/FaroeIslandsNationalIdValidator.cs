using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.FaroeIslands.Validators;

/// <summary>
/// Validator for Faroe Islands Personal Identification Number (P-tal).
/// Format: DDMMYY-XXX (9 digits).
/// </summary>
public class FaroeIslandsNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "FO";

    private static readonly int[] Weights = { 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <summary>
    /// Validates the Faroe Islands P-tal.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Faroe Islands P-tal (Static).
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

        if (sanitized.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Date Validation (DDMMYY)
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int yearPart = int.Parse(sanitized.Substring(4, 2));

        // Century handling is tricky with 2-digit year.
        // Assuming 1900-1999 for now, or sliding window?
        // P-tal system started in 1983.
        // Let's assume 1900+yearPart if yearPart > 20? Or just validate DD/MM.
        // We'll use a safe default century (e.g. 2000) just to validate day/month.
        int fullYear = 2000 + yearPart; 

        if (!DateHelper.IsValidDate(fullYear, month, day))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Checksum (Mod 11)
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (sanitized[i] - '0') * Weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = (remainder == 0) ? 0 : 11 - remainder;

        if (checkDigit == 11) checkDigit = 0; // Should not happen if remainder != 0
        // If remainder is 1, checkDigit is 10 -> Invalid P-tal.
        if (checkDigit == 10)
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        if (checkDigit != (sanitized[8] - '0'))
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
