using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Cyprus.Validators;

public class CyprusIbanValidator : IIbanValidator
{
    public string CountryCode => "CY";
    private const int CyprusIbanLength = 28;
    private const string CyprusCountryCode = "CY";

    public ValidationResult Validate(string? iban) => ValidateCyprusIban(iban);

    public static ValidationResult ValidateCyprusIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CyprusIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, CyprusIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(CyprusCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "CY"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = CyprusBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
