using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Sweden.Validators;

public class SwedenIbanValidator : IIbanValidator
{
    public string CountryCode => "SE";
    private const int SwedenIbanLength = 24;
    private const string SwedenCountryCode = "SE";

    public ValidationResult Validate(string? iban) => ValidateSwedenIban(iban);

    public static ValidationResult ValidateSwedenIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // 1. Length Check
        if (normalized.Length != SwedenIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SwedenIbanLength, normalized.Length));
        }

        // 2. Country Code Check
        if (!normalized.StartsWith(SwedenCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "SE"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = SwedenBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        // 4. ISO 7064 Mod 97 Checksum (The Gold Standard)
        if (!IbanHelper.IsValidIban(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        // We skip the complex Domestic Rules (Mod11/Luhn per Bank ID) to avoid false negatives.
        // The ISO Mod97 check guarantees the IBAN integrity sufficiently for international transfers.
        return ValidationResult.Success();
    }
}
