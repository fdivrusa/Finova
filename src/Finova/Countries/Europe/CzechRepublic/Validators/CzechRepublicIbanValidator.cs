using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicIbanValidator : IIbanValidator
{
    public string CountryCode => "CZ";
    private const int CzechIbanLength = 24;
    private const string CzechCountryCode = "CZ";

    public ValidationResult Validate(string? iban) => ValidateCzechIban(iban);

    public static ValidationResult ValidateCzechIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CzechIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, CzechIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(CzechCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "CZ"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = CzechRepublicBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
