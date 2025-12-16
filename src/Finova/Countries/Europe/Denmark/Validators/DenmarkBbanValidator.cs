using Finova.Core.Common;

namespace Finova.Countries.Europe.Denmark.Validators;

internal static class DenmarkBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 14 digits
        // Total length: 14 characters

        if (bban.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Digits only
        for (int i = 0; i < 14; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.DenmarkIbanMustContainOnlyDigits);
            }
        }

        return ValidationResult.Success();
    }
}
