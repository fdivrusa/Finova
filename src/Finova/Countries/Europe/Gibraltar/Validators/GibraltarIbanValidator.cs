using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Gibraltar.Validators;

public class GibraltarIbanValidator : IIbanValidator
{
    public string CountryCode => "GI";
    private const int GibraltarIbanLength = 23;
    private const string GibraltarCountryCode = "GI";

    public ValidationResult Validate(string? iban) => ValidateGibraltarIban(iban);

    public static ValidationResult ValidateGibraltarIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GibraltarIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, GibraltarIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(GibraltarCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidGibraltarCountryCode);
        }

        // Structure check:
        // Bank Code (4-8) is usually letters (BIC)
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GibraltarBankCodeMustBeLetters);
            }
        }

        // Account (8-23) is alphanumeric
        for (int i = 8; i < GibraltarIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.GibraltarAccountNumberMustBeAlphanumeric);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
