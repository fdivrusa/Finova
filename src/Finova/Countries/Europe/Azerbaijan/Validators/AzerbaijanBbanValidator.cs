using Finova.Core.Common;

namespace Finova.Countries.Europe.Azerbaijan.Validators;

internal static class AzerbaijanBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 4 letters (Bank) + 20 alphanumeric (Account)
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-4): 4 letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AzerbaijanIbanInvalidBankCode);
            }
        }

        // 2. Account Number (Pos 4-24): 20 alphanumeric
        for (int i = 4; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AzerbaijanIbanInvalidAccount);
            }
        }

        return ValidationResult.Success();
    }
}
