using Finova.Core.Common;

namespace Finova.Countries.Europe.Belarus.Validators;

internal static class BelarusBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        // BBAN format: 4 alphanumeric (Bank) + 20 alphanumeric (Account)
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Alphanumeric
        for (int i = 0; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanFormatAlphanumeric, "Belarus"));
            }
        }

        return ValidationResult.Success();
    }
}
