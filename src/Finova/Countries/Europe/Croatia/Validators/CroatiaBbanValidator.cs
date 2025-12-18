using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Croatia.Validators;

public class CroatiaBbanValidator : IBbanValidator
{
    public string CountryCode => "HR";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
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

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
