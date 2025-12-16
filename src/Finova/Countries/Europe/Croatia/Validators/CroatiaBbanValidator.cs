using Finova.Core.Common;

namespace Finova.Countries.Europe.Croatia.Validators;

internal static class CroatiaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 17 digits
        // Total length: 17 characters

        if (bban.Length != 17)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Digits only
        for (int i = 0; i < 17; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Croatia"));
            }
        }

        return ValidationResult.Success();
    }
}
