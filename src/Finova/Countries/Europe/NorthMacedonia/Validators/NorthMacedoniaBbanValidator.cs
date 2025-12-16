using Finova.Core.Common;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

public static class NorthMacedoniaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 3 digits (Bank) + 10 alphanumeric (Account) + 2 digits (Check)
        // Total length: 15 characters

        if (bban.Length != 15)
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

        // 2. Account Number (Pos 3-13): 10 alphanumeric characters
        for (int i = 3; i < 13; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
            }
        }

        // 3. National Check Digits (Pos 13-15): 2 digits
        for (int i = 13; i < 15; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NationalCheckDigitsMustBeNumeric);
            }
        }

        return ValidationResult.Success();
    }
}
