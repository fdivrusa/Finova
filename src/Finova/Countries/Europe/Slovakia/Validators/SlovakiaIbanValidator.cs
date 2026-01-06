using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Slovakia.Validators;

public class SlovakiaIbanValidator : IIbanValidator
{
    public string CountryCode => "SK";
    private const int SlovakiaIbanLength = 24;
    private const string SlovakiaCountryCode = "SK";

    public ValidationResult Validate(string? iban) => ValidateSlovakiaIban(iban);

    public static ValidationResult ValidateSlovakiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SlovakiaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SlovakiaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(SlovakiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "SK"));
        }

        // --- Specific SK Validation (Modulo 11) ---
        // BBAN is 20 digits (from index 4)
        string bban = normalized.Substring(4, 20);

        var bbanResult = SlovakiaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
