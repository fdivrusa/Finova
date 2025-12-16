using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Iceland.Validators;

public class IcelandIbanValidator : IIbanValidator
{
    public string CountryCode => "IS";
    private const int IcelandIbanLength = 26;
    private const string IcelandCountryCode = "IS";

    public ValidationResult Validate(string? iban) => ValidateIcelandIban(iban);

    public static ValidationResult ValidateIcelandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != IcelandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, IcelandIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(IcelandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidIcelandCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = IcelandBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
