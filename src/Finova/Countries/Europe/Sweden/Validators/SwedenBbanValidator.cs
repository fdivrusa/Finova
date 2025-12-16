using Finova.Core.Common;

namespace Finova.Countries.Europe.Sweden.Validators;

public static class SwedenBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 20 digits
        // Total length: 20 characters

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Digits only
        for (int i = 0; i < 20; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Sweden"));
            }
        }

        return ValidationResult.Success();
    }
}
