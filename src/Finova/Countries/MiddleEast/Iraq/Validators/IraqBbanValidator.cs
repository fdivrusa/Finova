using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.Iraq.Validators;

/// <summary>
/// Validator for Iraq BBAN.
/// Format: 19 characters (4 letters Bank, 3 digits Branch, 12 digits Account).
/// </summary>
public class IraqBbanValidator : IBbanValidator
{
    public string CountryCode => "IQ";
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

        // Format: 4 letters (Bank), 3 digits (Branch), 12 digits (Account)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeAlphanumeric);
        }

        if (!bban.Substring(4, 3).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (!bban.Substring(7, 12).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeNumeric);
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
