using Finova.Core.Common;

namespace Finova.Countries.Europe.Vatican.Validators;

public static class VaticanBbanValidator
{
    public static ValidationResult Validate(string bban)
    {
        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        for (int i = 0; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidVaticanIbanFormat);
            }
        }

        return ValidationResult.Success();
    }
}
