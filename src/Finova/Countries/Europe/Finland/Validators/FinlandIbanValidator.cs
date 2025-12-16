using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

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

        // Finland IBANs are strictly numeric after the country code.
        for (int i = 2; i < FinlandIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Finland"));
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
