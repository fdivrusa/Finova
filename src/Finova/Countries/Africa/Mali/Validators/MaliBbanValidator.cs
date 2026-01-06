using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Mali.Validators;

/// <summary>
/// Validator for Mali BBAN.
/// Format: 24 characters (1 letter + 23 digits).
/// </summary>
public class MaliBbanValidator : IBbanValidator
{
    public string CountryCode => "ML";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 24)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 24, bban.Length));
        }

        if (!char.IsLetter(bban[0]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BbanMustStartWithLetter);
        }

        for (int i = 1; i < 24; i++)
        {
            if (!char.IsDigit(bban[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BbanMustContainDigitsAfterLetter);
            }
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
