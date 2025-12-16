using Finova.Core.Common;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

public static class LiechtensteinBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 5 digits (Bank) + 12 alphanumeric (Account)
        // Total length: 17 characters

        if (bban.Length != 17)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-5): 5 digits
        for (int i = 0; i < 5; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeNumeric);
            }
        }

        // 2. Account Number (Pos 5-17): 12 alphanumeric characters
        for (int i = 5; i < 17; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
            }
        }

        return ValidationResult.Success();
    }
}
