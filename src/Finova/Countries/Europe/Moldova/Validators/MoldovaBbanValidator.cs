using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Moldova.Validators;

public class MoldovaBbanValidator : IBbanValidator
{
    public string CountryCode => "MD";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BBAN format: 2 alphanumeric (Bank) + 18 alphanumeric (Account)
        // Total length: 20 characters

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank Code (Pos 0-2): 2 alphanumeric characters
        for (int i = 0; i < 2; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeAlphanumeric);
            }
        }

        // 2. Account Number (Pos 2-20): 18 alphanumeric characters
        for (int i = 2; i < 20; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
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
