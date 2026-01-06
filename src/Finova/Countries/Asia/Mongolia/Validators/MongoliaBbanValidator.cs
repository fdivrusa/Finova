using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Mongolia.Validators;

/// <summary>
/// Validator for Mongolia BBAN.
/// Format: 16 digits.
/// </summary>
public class MongoliaBbanValidator : IBbanValidator
{
    public string CountryCode => "MN";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 16, bban.Length));
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
