using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.Palestine.Validators;

/// <summary>
/// Validator for Palestine BBAN.
/// Format: 25 characters (4 letters Bank, 21 alphanumeric Account).
/// </summary>
public class PalestineBbanValidator : IBbanValidator
{
    public string CountryCode => "PS";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 25, bban.Length));
        }

        // Format: 4 letters (Bank), 21 alphanumeric (Account)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeAlphanumeric);
        }

        if (!bban.Substring(4).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
