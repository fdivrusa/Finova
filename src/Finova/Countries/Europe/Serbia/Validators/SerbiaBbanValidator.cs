using Finova.Core.Common;

namespace Finova.Countries.Europe.Serbia.Validators;

public static class SerbiaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 3 digits (Bank) + 13 digits (Account) + 2 digits (Check)
        // Total length: 18 characters

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: All digits
        for (int i = 0; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSerbiaIbanFormat);
            }
        }

        return ValidationResult.Success();
    }
}
