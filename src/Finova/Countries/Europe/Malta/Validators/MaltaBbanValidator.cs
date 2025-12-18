using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Malta.Validators;

public class MaltaBbanValidator : IBbanValidator
{
    public string CountryCode => "MT";
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

        // BBAN format: 4 letters (Bank) + 5 digits (Sort Code) + 18 alphanumeric (Account)
        // Total length: 27 characters

        if (bban.Length != 27)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // 1. Bank BIC (Pos 0-4): Must be letters
        for (int i = 0; i < 4; i++)
        {
            if (!char.IsLetter(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MaltaBankBicMustBeLetters);
            }
        }

        // 2. Sort Code (Pos 4-9): Must be digits
        for (int i = 4; i < 9; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MaltaSortCodeMustBeDigits);
            }
        }

        // 3. Account Number (Pos 9-27): Alphanumeric
        for (int i = 9; i < 27; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MaltaAccountNumberMustBeAlphanumeric);
            }
        }

        return ValidationResult.Success();
    }
}
