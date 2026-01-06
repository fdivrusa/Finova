using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Finland.Validators;

public class FinlandIbanValidator : IIbanValidator
{
    public string CountryCode => "FI";
    private const int FinlandIbanLength = 18;

    public ValidationResult Validate(string? iban) => ValidateFinlandIban(iban);

    public static ValidationResult ValidateFinlandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != FinlandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, FinlandIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith("FI", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "FI"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = FinlandBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
