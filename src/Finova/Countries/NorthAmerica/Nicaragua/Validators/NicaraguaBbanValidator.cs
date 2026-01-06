using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Nicaragua.Validators;

/// <summary>
/// Validator for Nicaragua BBAN.
/// Format: 28 characters (4 letters Bank, 24 digits Account).
/// </summary>
public class NicaraguaBbanValidator : IBbanValidator
{
    public string CountryCode => "NI";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    public static ValidationResult Validate(string? bban)
    {
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 28)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 28, bban.Length));
        }

        // Format: 4 letters (Bank), 24 digits (Account)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeAlphanumeric);
        }

        if (!bban.Substring(4, 24).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeNumeric);
        }

        return ValidationResult.Success();
    }

    public string? Parse(string? input) => Validate(input).IsValid ? input : null;
}
