using Finova.Core.Common;

namespace Finova.Countries.Europe.Romania.Validators;

public static class RomaniaBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 4 alphanumeric (Bank) + 16 alphanumeric (Account)
        // Total length: 20 characters

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Alphanumeric
        // Bank Code (Pos 0-4) and Account Number (Pos 4-20) can contain letters.
        for (int i = 0; i < 20; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidRomaniaIbanFormat);
            }
        }

        return ValidationResult.Success();
    }
}
