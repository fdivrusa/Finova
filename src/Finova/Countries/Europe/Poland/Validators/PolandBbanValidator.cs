using Finova.Core.Common;

namespace Finova.Countries.Europe.Poland.Validators;

public static class PolandBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 24 digits
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: All digits
        for (int i = 0; i < 24; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Poland"));
            }
        }

        return ValidationResult.Success();
    }
}
