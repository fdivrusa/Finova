using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Cameroon.Validators;

/// <summary>
/// Validator for Cameroon BBAN.
/// Format: 23 digits.
/// </summary>
public class CameroonBbanValidator : IBbanValidator
{
    public string CountryCode => "CM";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 23)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 23, bban.Length));
        }

        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
            }
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
