using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Slovenia.Validators;

public class SloveniaIbanValidator : IIbanValidator
{
    public string CountryCode => "SI";
    private const int SloveniaIbanLength = 19;
    private const string SloveniaCountryCode = "SI";

    public ValidationResult Validate(string? iban) => ValidateSloveniaIban(iban);

    public static ValidationResult ValidateSloveniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SloveniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SloveniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(SloveniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "SI"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = SloveniaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        // Note: Slovenia uses Mod 97 for the internal BBAN (last 15 digits).
        // Since the IBAN uses Mod 97 globally, strictly validating the IBAN envelope 
        // covers the internal integrity.

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
