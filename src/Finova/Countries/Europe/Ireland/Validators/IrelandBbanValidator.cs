using Finova.Core.Common;

namespace Finova.Countries.Europe.Ireland.Validators;

public static class IrelandBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
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
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IrelandBankCodeMustBeLetters);
            }
        }

        // 2. Sort Code (Pos 4-10): 6 digits
        for (int i = 4; i < 10; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IrelandSortCodeAccountNumberMustBeDigits);
            }
        }

        // 3. Account Number (Pos 10-18): 8 digits
        for (int i = 10; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.IrelandSortCodeAccountNumberMustBeDigits);
            }
        }

        return ValidationResult.Success();
    }
}
