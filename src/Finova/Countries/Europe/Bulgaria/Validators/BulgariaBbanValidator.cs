using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Bulgaria.Validators;

public class BulgariaBbanValidator : IBbanValidator
{
    public string CountryCode => "BG";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // BBAN format: 4 letters (Bank) + 4 digits (Branch) + 2 digits (Account Type) + 8 alphanumeric (Account)
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
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BulgariaIbanInvalidBankCode);
            }
        }

        // 2. Branch Code (Pos 4-8): 4 digits
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bulgaria Branch Code must be digits.");
            }
        }

        // 3. Account Type (Pos 8-10): 2 digits
        for (int i = 8; i < 10; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bulgaria Account Type must be digits.");
            }
        }

        // 4. Account Number (Pos 10-18): 8 alphanumeric
        for (int i = 10; i < 18; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Bulgaria"));
            }
        }

        return ValidationResult.Success();
    }
}
