using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Romania.Validators;

public class RomaniaBbanValidator : IBbanValidator
{
    public string CountryCode => "RO";
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

        // BBAN format: 4 alphanumeric (Bank) + 16 alphanumeric (Account)
        // Total length: 20 characters

        if (bban.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Invalid BBAN length.");
        }

        // Structure check: Alphanumeric
        // Bank Code (Pos 0-4) and Account Number (Pos 4-20) can contain letters.
        for (int i = 0; i < 20; i++)
        {
            if (!char.IsLetterOrDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidRomaniaIbanFormat);
            }
        }

        return ValidationResult.Success();
    }
}
