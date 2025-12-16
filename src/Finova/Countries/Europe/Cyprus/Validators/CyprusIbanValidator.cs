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

        // Structure Validation:

        // 1. Bank Code (Pos 4-7): Must be digits
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusIbanInvalidBankCode);
            }
        }

        // 2. Branch Code (Pos 7-12): Must be digits
        for (int i = 7; i < 12; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusIbanInvalidBranchCode);
            }
        }

        // 3. Account Number (Pos 12-28): Alphanumeric
        for (int i = 12; i < 28; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CyprusIbanInvalidAccount);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
