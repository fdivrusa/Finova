using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Sweden.Validators;

/// <summary>
/// Validator for Sweden Personal Identity Number (Personnummer).
/// </summary>
public partial class SwedenPersonnummerValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "SE";

    /// <summary>
    /// Validates the Swedish Personnummer.
    /// </summary>
    /// <param name="nationalId">The Personnummer to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Swedish Personnummer (Static).
    /// </summary>
    /// <param name="nationalId">The Personnummer to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Normalize: Remove spaces, hyphens, plus signs
        string normalized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        // Handle 12-digit format (YYYYMMDDXXXX) -> convert to 10 digits (YYMMDDXXXX)
        if (normalized.Length == 12)
        {
            normalized = normalized.Substring(2);
        }

        if (normalized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(normalized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Date (YYMMDD)
        if (!IsValidDate(normalized.Substring(0, 6)))
        {
             // Coordination numbers (Samordningsnummer) add 60 to the day
             if (!IsValidCoordinationDate(normalized.Substring(0, 6)))
             {
                 return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid date part.");
             }
        }

        // Luhn Check
        if (!ChecksumHelper.ValidateLuhn(normalized))
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

    private static bool IsValidDate(string yymmdd)
    {
        // Simple check, ideally use DateTime.TryParseExact
        // But we don't know the century for sure without the separator context if input was 10 digits.
        // Assuming standard date validation.
        int year = int.Parse(yymmdd.Substring(0, 2));
        int month = int.Parse(yymmdd.Substring(2, 2));
        int day = int.Parse(yymmdd.Substring(4, 2));

        if (month < 1 || month > 12) return false;
        if (day < 1 || day > 31) return false;
        
        return true;
    }

    private static bool IsValidCoordinationDate(string yymmdd)
    {
        int year = int.Parse(yymmdd.Substring(0, 2));
        int month = int.Parse(yymmdd.Substring(2, 2));
        int day = int.Parse(yymmdd.Substring(4, 2));

        // Coordination numbers: Day is between 61 and 91
        if (month < 1 || month > 12) return false;
        if (day < 61 || day > 91) return false;

        return true;
    }
}
