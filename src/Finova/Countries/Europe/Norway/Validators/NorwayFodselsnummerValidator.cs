using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norway National Identity Number (Fødselsnummer).
/// </summary>
public class NorwayFodselsnummerValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "NO";

    private static readonly int[] Weights1 = { 3, 7, 6, 1, 8, 9, 4, 5, 2 };
    private static readonly int[] Weights2 = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    /// <summary>
    /// Validates the Norwegian Fødselsnummer.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Norwegian Fødselsnummer (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string normalized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(normalized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Date (DDMMYY)
        // D-numbers add 4 to the first digit of the day (Day + 40)
        int day = int.Parse(normalized.Substring(0, 2));
        int month = int.Parse(normalized.Substring(2, 2));
        
        bool isDNumber = day > 40;
        if (isDNumber) day -= 40;

        // H-numbers add 4 to the first digit of the month (Month + 40)
        bool isHNumber = month > 40;
        if (isHNumber) month -= 40;

        if (month < 1 || month > 12 || day < 1 || day > 31)
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid date part.");
        }

        // Checksum 1 (10th digit)
        int remainder1 = ChecksumHelper.CalculateWeightedModulo11(normalized.Substring(0, 9), Weights1);
        int checkDigit1 = remainder1 == 0 ? 0 : 11 - remainder1;
        
        if (checkDigit1 == 10) return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        
        if (checkDigit1 != (normalized[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // Checksum 2 (11th digit)
        int remainder2 = ChecksumHelper.CalculateWeightedModulo11(normalized.Substring(0, 10), Weights2);
        int checkDigit2 = remainder2 == 0 ? 0 : 11 - remainder2;

        if (checkDigit2 == 10) return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);

        if (checkDigit2 != (normalized[10] - '0'))
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
