using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Austria.Validators;

public class AustriaIbanValidator : IIbanValidator
{
    public string CountryCode => "AT";
    private const int AustriaIbanLength = 20;

    public ValidationResult Validate(string? iban) => ValidateAustriaIban(iban);

    public static ValidationResult ValidateAustriaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AustriaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {AustriaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected AT.");
        }

        // Austria IBANs are strictly numeric after the country code.
        // Positions 2 to 19 (Check digits + BLZ + Kontonummer)
        for (int i = 2; i < AustriaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Austria IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
