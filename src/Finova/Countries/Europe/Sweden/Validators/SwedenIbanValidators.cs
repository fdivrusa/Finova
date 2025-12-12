using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Sweden.Validators;

public class SwedenIbanValidator : IIbanValidator
{
    public string CountryCode => "SE";
    private const int SwedenIbanLength = 24;
    private const string SwedenCountryCode = "SE";

    public ValidationResult Validate(string? iban) => ValidateSwedenIban(iban);

    public static ValidationResult ValidateSwedenIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // 1. Length Check
        if (normalized.Length != SwedenIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {SwedenIbanLength}, got {normalized.Length}.");
        }

        // 2. Country Code Check
        if (!normalized.StartsWith(SwedenCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected SE.");
        }

        // 3. Format Check (Digits only after SE)
        for (int i = 2; i < SwedenIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Sweden IBAN must contain only digits after country code.");
            }
        }

        // 4. ISO 7064 Mod 97 Checksum (The Gold Standard)
        if (!IbanHelper.IsValidIban(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
        }

        // We skip the complex Domestic Rules (Mod11/Luhn per Bank ID) to avoid false negatives.
        // The ISO Mod97 check guarantees the IBAN integrity sufficiently for international transfers.
        return ValidationResult.Success();
    }
}
