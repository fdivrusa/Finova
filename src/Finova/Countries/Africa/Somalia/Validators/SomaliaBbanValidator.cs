using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Somalia.Validators;

/// <summary>
/// Validator for Somalia BBAN.
/// Format: 19 digits.
/// </summary>
public class SomaliaBbanValidator : IBbanValidator
{
    public string CountryCode => "SO";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 19)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 19, bban.Length));
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
