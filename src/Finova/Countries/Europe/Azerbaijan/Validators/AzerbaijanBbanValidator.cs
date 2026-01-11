using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Azerbaijan.Validators;

public class AzerbaijanBbanValidator : IBbanValidator
{
    public string CountryCode => "AZ";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        // BBAN format: 4 letters (Bank) + 20 alphanumeric (Account)
        // Total length: 24 characters

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidBbanLength);
        }

        // 1. Bank Code (Pos 0-4): 4 letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AzerbaijanIbanInvalidBankCode);
            }
        }

        // 2. Account Number (Pos 4-24): 20 alphanumeric
        for (int i = 4; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AzerbaijanIbanInvalidAccount);
            }
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        var sanitized = InputSanitizer.Sanitize(bban);
        if (string.IsNullOrWhiteSpace(sanitized))
        {
            return null;
        }

        var result = Validate(sanitized);
        if (!result.IsValid)
        {
            return null;
        }

        return new BbanDetails
        {
            Bban = sanitized,
            CountryCode = CountryCode,
            BankCode = sanitized[..4],
            AccountNumber = sanitized.Substring(4)
        };
    }
}
