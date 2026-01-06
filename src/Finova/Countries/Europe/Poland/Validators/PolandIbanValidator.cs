using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Poland.Validators;

public class PolandIbanValidator : IIbanValidator
{
    public string CountryCode => "PL";
    private const int PolandIbanLength = 28;
    private const string PolandCountryCode = "PL";

    public ValidationResult Validate(string? iban) => ValidatePolandIban(iban);

    public static ValidationResult ValidatePolandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != PolandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, PolandIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(PolandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "PL"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = PolandBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        // Note: Poland uses an internal Modulo 97 on the BBAN (last 24 digits).
        // The global IBAN validation ensures this mathematically.
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
