using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Montenegro.Validators;

public class MontenegroBbanValidator : IBbanValidator
{
    public string CountryCode => "ME";
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

        // BBAN format: 3 digits (Bank) + 13 digits (Account) + 2 digits (Check)
        // Total length: 18 characters

        if (bban.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-3): 3 digits
        for (int i = 0; i < 3; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeNumeric);
            }
        }

        // 2. Account Number (Pos 3-16: 13 digits
        for (int i = 3; i < 16; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeNumeric);
            }
        }

        // 3. National Check Digits (Pos 16-18): 2 digits
        for (int i = 16; i < 18; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NationalCheckDigitsMustBeNumeric);
            }
        }

        return ValidationResult.Success();
    }
}
