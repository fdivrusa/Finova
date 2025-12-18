using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

public class UnitedKingdomBbanValidator : IBbanValidator
{
    public string CountryCode => "GB";

    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 4 letters (Bank) + 6 digits (Sort Code) + 8 digits (Account)
        // Total length: 18 characters

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-4): 4 letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkBankCodeFormat);
            }
        }

        // 2. Sort Code (Pos 4-10): 6 digits
        for (int i = 4; i < 10; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkSortCodeFormat);
            }
        }

        // 3. Account Number (Pos 10-18): 8 digits (implied by remaining length check in IBAN validator, but good to be explicit here if we want full BBAN validation)
        // The original code checked 8-21 (14 chars) which is Sort Code (6) + Account (8).
        // Wait, original code:
        // Check that positions 8 to 21 are DIGITS.
        // Pos 8 in IBAN is index 4 in BBAN.
        // So BBAN index 4 to 18 must be digits.
        for (int i = 10; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                // Reusing sort code message or generic format message?
                // The original code grouped sort code and account number check.
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkSortCodeFormat); 
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "").Replace("-", "").Trim();
        return Validate(sanitized).IsValid ? sanitized : null;
    }
}
