using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Africa.Somalia.Validators;

/// <summary>
/// Validator for Somalia IBANs.
/// Somalia IBAN format: SO + 2 check digits + 19 digits BBAN.
/// Length: 23 characters.
/// </summary>
public class SomaliaIbanValidator : IIbanValidator
{
    public string CountryCode => "SO";
    private const int IbanLength = 23;
    private const string CountryCodeVal = "SO";

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

        var bbanResult = SomaliaBbanValidator.Validate(normalized.Substring(4));
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized) ? ValidationResult.Success() : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
