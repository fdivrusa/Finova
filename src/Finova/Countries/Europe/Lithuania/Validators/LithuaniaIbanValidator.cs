using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Lithuania.Validators;

public class LithuaniaIbanValidator : IIbanValidator
{
    public string CountryCode => "LT";
    private const int LithuaniaIbanLength = 20;
    private const string LithuaniaCountryCode = "LT";

    public ValidationResult Validate(string? iban) => ValidateLithuaniaIban(iban);

    public static ValidationResult ValidateLithuaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LithuaniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, LithuaniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(LithuaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "LT"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = LithuaniaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
