using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.MiddleEast.Iraq.Validators;

/// <summary>
/// Validator for Iraq IBANs.
/// Iraq IBAN format: IQ + 2 check digits + 19 characters BBAN (4 letters bank, 3 digits branch, 12 digits account).
/// Length: 23 characters.
/// </summary>
public class IraqIbanValidator : IIbanValidator
{
    public string CountryCode => "IQ";
    private const int IbanLength = 23;
    private const string CountryCodeVal = "IQ";

    public ValidationResult Validate(string? iban) => ValidateIban(iban);

    public static ValidationResult ValidateIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != IbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, IbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(CountryCodeVal, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        var bbanResult = IraqBbanValidator.Validate(normalized.Substring(4));
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized) ? ValidationResult.Success() : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
