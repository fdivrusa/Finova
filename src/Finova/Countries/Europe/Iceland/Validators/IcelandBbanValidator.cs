using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Iceland.Validators;

/// <summary>
/// Validator for Icelandic BBAN (Basic Bank Account Number).
/// </summary>
public class IcelandBbanValidator : IBbanValidator
{
    public string CountryCode => "IS";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Icelandic National ID (Kennitala).
    /// Uses Modulo 11 with weights: 3, 2, 7, 6, 5, 4, 3, 2.
    /// The 9th digit is the check digit.
    /// </summary>
    /// <param name="kt">The Kennitala to validate.</param>
    /// <returns>True if valid, false otherwise.</returns>
    public static bool ValidateKennitala(string kt)
    {
        if (string.IsNullOrEmpty(kt) || kt.Length != 10)
        {
            return false;
        }

        // Kennitala weights for the first 8 digits
        int[] weights = [3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            if (!char.IsDigit(kt[i]))
            {
                return false;
            }
            sum += (kt[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 0 ? 0 : 11 - remainder;

        if (checkDigit == 10)
        {
            return false; // Invalid Kennitala
        }

        // The 9th digit (index 8) is the control digit
        return checkDigit == (kt[8] - '0');
    }

    /// <summary>
    /// Validates the Icelandic BBAN.
    /// </summary>
    /// <param name="bban">The BBAN to validate.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 22)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 22, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IcelandIbanMustContainOnlyDigits);
            }
        }

        // Internal Validation: Kennitala (National ID)
        // The last 10 digits (Pos 12-22 of BBAN) are the Kennitala.
        string kennitala = bban.Substring(12, 10);
        if (!ValidateKennitala(kennitala))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIcelandKennitala);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
