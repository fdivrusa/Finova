using Finova.Core.Common;

namespace Finova.Countries.Europe.Andorra.Validators;

internal static class AndorraBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 8 alphanumeric (Bank + Branch) + 12 alphanumeric (Account)
        // Total length: 20 characters

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Alphanumeric
        for (int i = 0; i < 20; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Andorra"));
            }
        }

        return ValidationResult.Success();
    }
}
