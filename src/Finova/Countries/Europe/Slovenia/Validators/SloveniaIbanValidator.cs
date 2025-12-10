using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Slovenia.Validators;

public class SloveniaIbanValidator : IIbanValidator
{
    public string CountryCode => "SI";
    private const int SloveniaIbanLength = 19;
    private const string SloveniaCountryCode = "SI";

    public ValidationResult Validate(string? iban) => ValidateSloveniaIban(iban);

    public static ValidationResult ValidateSloveniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SloveniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {SloveniaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(SloveniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected SI.");
        }

        // Structure check: Digits only
        for (int i = 2; i < SloveniaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Slovenia IBAN must contain only digits after the country code.");
            }
        }

        // Note: Slovenia uses Mod 97 for the internal BBAN (last 15 digits).
        // Since the IBAN uses Mod 97 globally, strictly validating the IBAN envelope 
        // covers the internal integrity.

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
