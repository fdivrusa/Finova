using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Croatia.Validators;

public class CroatiaIbanValidator : IIbanValidator
{
    public string CountryCode => "HR";
    private const int CroatiaIbanLength = 21;
    private const string CroatiaCountryCode = "HR";

    public ValidationResult Validate(string? iban) => ValidateCroatiaIban(iban);

    public static ValidationResult ValidateCroatiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CroatiaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, CroatiaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(CroatiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "HR"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = CroatiaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
