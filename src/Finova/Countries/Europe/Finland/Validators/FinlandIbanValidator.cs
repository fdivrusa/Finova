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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != FinlandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {FinlandIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith("FI", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected FI.");
        }

        // Finland IBANs are strictly numeric after the country code.
        for (int i = 2; i < FinlandIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Finland IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
