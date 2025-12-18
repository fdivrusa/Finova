using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ukraine.Validators;

public class UkraineBbanValidator : IBbanValidator
{
    public string CountryCode => "UA";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 6 digits (Bank) + 19 digits (Account)
        // Total length: 25 characters

        if (bban.Length != 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (MFO) (Pos 0-6): 6 digits
        for (int i = 0; i < 6; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkraineBankCodeFormat);
            }
        }

        // 2. Account Number (Pos 6-25): 19 digits
        for (int i = 6; i < 25; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidUkraineAccountNumberFormat);
            }
        }

        return ValidationResult.Success();
    }
}
