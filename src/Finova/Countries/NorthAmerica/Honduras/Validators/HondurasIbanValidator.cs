using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.Honduras.Validators;

/// <summary>
/// Validator for Honduras IBANs.
/// Honduras IBAN format: HN + 2 check digits + 24 characters BBAN (4 letters bank, 20 digits account).
/// Length: 28 characters.
/// </summary>
public class HondurasIbanValidator : IIbanValidator
{
    public string CountryCode => "HN";
    private const int IbanLength = 28;
    private const string CountryCodeVal = "HN";

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

        var bbanResult = HondurasBbanValidator.Validate(normalized.Substring(4));
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized) ? ValidationResult.Success() : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
