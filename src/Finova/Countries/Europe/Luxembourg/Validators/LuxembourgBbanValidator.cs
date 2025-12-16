using Finova.Core.Common;

namespace Finova.Countries.Europe.Luxembourg.Validators;

public static class LuxembourgBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 3 digits (Bank) + 13 alphanumeric (Account)
        // Total length: 16 characters

        if (bban.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Bank Code (indices 0-3) must be numeric.
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLuxembourgBankCode);
            }
        }

        return ValidationResult.Success();
    }
}
