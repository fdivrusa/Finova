using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

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

        // Structure check: Digits only
        for (int i = 2; i < CroatiaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Croatia"));
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
