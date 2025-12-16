using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Denmark.Validators;

public class DenmarkIbanValidator : IIbanValidator
{
    public string CountryCode => "DK";
    private const int DenmarkIbanLength = 18;
    private const string DenmarkCountryCode = "DK";

    public ValidationResult Validate(string? iban) => ValidateDenmarkIban(iban);

    public static ValidationResult ValidateDenmarkIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != DenmarkIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, DenmarkIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(DenmarkCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidDenmarkCountryCode);
        }

        // Structure check: All digits
        for (int i = 2; i < DenmarkIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.DenmarkIbanMustContainOnlyDigits);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
