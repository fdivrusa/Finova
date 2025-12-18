using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

public class NorthMacedoniaBbanValidator : IBbanValidator
{
    public string CountryCode => "MK";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 3 digits (Bank) + 10 alphanumeric (Account) + 2 digits (Check)
        // Total length: 15 characters

        if (bban.Length != 15)
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

        // 2. Account Number (Pos 3-13): 10 alphanumeric characters
        for (int i = 3; i < 13; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
            }
        }

        // 3. National Check Digits (Pos 13-15): 2 digits
        for (int i = 13; i < 15; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NationalCheckDigitsMustBeNumeric);
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
