using Finova.Core.Common;

namespace Finova.Countries.Europe.Montenegro.Validators;

public static class MontenegroBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 3 digits (Bank) + 13 digits (Account) + 2 digits (Check)
        // Total length: 18 characters

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-3): 3 digits
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeNumeric);
            }
        }

        // 2. Account Number (Pos 3-16): 13 digits
        for (int i = 3; i < 16; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeNumeric);
            }
        }

        // 3. National Check Digits (Pos 16-18): 2 digits
        for (int i = 16; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NationalCheckDigitsMustBeNumeric);
            }
        }

        return ValidationResult.Success();
    }
}
