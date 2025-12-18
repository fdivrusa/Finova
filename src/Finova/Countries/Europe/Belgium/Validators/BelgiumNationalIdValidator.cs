using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validates the Belgian National Number (Numéro de registre national / Rijksregisternummer).
/// </summary>
public class BelgiumNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "BE";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the Belgian National ID format, date logic, and checksum.
    /// </summary>
    /// <param name="input">The National ID to validate.</param>
    /// <returns>The validation result.</returns>
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

        if (sanitized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Split into Number (first 9 digits) and Checksum (last 2 digits)
        if (!long.TryParse(sanitized[..9], out long number) || !long.TryParse(sanitized[9..], out long checksum))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate the date part (YYMMDD), accounting for Bis/Ter numbers
        if (!IsValidBelgianDate(sanitized[..6]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid birth date in National Number.");
        }

        // Standard Check: Individuals born before 2000
        // Formula: 97 - (Number % 97)
        long calculatedChecksum = 97 - (number % 97);
        if (calculatedChecksum == checksum)
        {
            return ValidationResult.Success();
        }

        // 2000+ Check: Individuals born in or after 2000
        // Mathematically equivalent to prepending '2' to the string (adding 2,000,000,000)
        long number2000 = number + 2000000000L;
        long calculatedChecksum2000 = 97 - (number2000 % 97);

        if (calculatedChecksum2000 == checksum)
        {
            return ValidationResult.Success();
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    /// <summary>
    /// Validates the YYMMDD part, accounting for Bis-numbers (month + 20) and Ter-numbers (month + 40).
    /// </summary>
    private static bool IsValidBelgianDate(string datePart)
    {
        // We know these parses will succeed because of the previous long.TryParse check on the whole string
        int month = int.Parse(datePart[2..4]);
        int day = int.Parse(datePart[4..]);

        // Handle Special Registers (Bis/Ter numbers)
        // 01-12: Normal
        // 21-32: Bis (gender changes or foreigners)
        // 41-52: Ter (rare)
        if (month > 40)
        {
            month -= 40;
        }
        else if (month > 20)
        {
            month -= 20;
        }

        // Basic month/day validation
        // 0 is valid for both month and day, as it may indicate unknown values
        if (month > 12)
        {
            return false;
        }

        if (day > 31)
        {
            return false;
        }

        return true;
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
