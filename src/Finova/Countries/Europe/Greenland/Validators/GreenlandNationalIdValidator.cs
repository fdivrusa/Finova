using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greenland.Validators;

/// <summary>
/// Validator for Greenland Personal Identification Number (CPR-nummer).
/// Format: DDMMYY-XXXX (10 digits).
/// Same format as Denmark.
/// </summary>
public class GreenlandNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "GL";

    /// <summary>
    /// Validates the Greenland CPR number.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Greenland CPR number (Static).
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

        // Date Validation (DDMMYY)
        int day = int.Parse(sanitized.Substring(0, 2));
        int month = int.Parse(sanitized.Substring(2, 2));
        int yearPart = int.Parse(sanitized.Substring(4, 2));

        // Century handling (similar to Denmark)
        // For simplicity, we'll just validate DD/MM and assume a valid year exists.
        // Denmark has complex century logic based on the 7th digit.
        // We can implement basic date check here.
        
        // Basic check: Month 1-12, Day 1-31.
        if (month < 1 || month > 12 || day < 1 || day > 31)
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Mod 11 Checksum (Classic CPR)
        // Note: Denmark stopped issuing Mod 11 compliant numbers in 2007 for some cases, but Greenland might still use it.
        // However, to be safe and support all valid numbers (including those without Mod 11 if applicable), 
        // we might skip strict Mod 11 or make it optional.
        // But usually legacy systems enforce it.
        // Let's implement Mod 11 check as it's standard for CPR.
        
        int[] weights = { 4, 3, 2, 7, 6, 5, 4, 3, 2, 1 };
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (sanitized[i] - '0') * weights[i];
        }

        if (sum % 11 != 0)
        {
            // Some valid CPR numbers do not satisfy Mod 11 (since 2007 in DK).
            // If Greenland follows DK exactly, we should allow non-Mod 11.
            // But if we want to be strict for "classic" numbers...
            // Let's assume strict for now, but if users complain, we relax it.
            // Actually, for a library, it's better to be correct.
            // "Since 2007, the modulus 11 check is no longer valid for all CPR numbers."
            // So we should NOT enforce Mod 11 if we want to support all valid IDs.
            // But we should validate the date.
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
