using Finova.Core.Common;

namespace Finova.Countries.Europe.Austria.Validators;

internal static class AustriaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 16 digits (BLZ + Kontonummer)
        // Total length: 16 characters

        if (bban.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Digits only
        for (int i = 0; i < 16; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Austria"));
            }
        }

        return ValidationResult.Success();
    }
}
