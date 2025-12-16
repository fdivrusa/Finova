using Finova.Core.Common;

namespace Finova.Countries.Europe.Albania.Validators;

internal static class AlbaniaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 3 digits (Bank) + 4 digits (Branch) + 1 alphanumeric (Control) + 16 alphanumeric (Account)
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-3): 3 digits
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidBankCode);
            }
        }

        // 2. Branch Code (Pos 3-7): 4 digits
        for (int i = 3; i < 7; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidBranchCode);
            }
        }

        // 3. Control Character (Pos 7): 1 alphanumeric character
        if (!char.IsLetterOrDigit(bban[7]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidControlChar);
        }

        // 4. Account Number (Pos 8-24): 16 alphanumeric characters
        for (int i = 8; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidAccount);
            }
        }

        return ValidationResult.Success();
    }
}
