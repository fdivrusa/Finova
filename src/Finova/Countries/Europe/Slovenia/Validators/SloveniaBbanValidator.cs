using Finova.Core.Common;

namespace Finova.Countries.Europe.Slovenia.Validators;

public static class SloveniaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 5 digits (Bank) + 8 digits (Account) + 2 digits (Check)
        // Total length: 15 characters

        if (bban.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Digits only
        for (int i = 0; i < 15; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Slovenia"));
            }
        }

        return ValidationResult.Success();
    }
}
