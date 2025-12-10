using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Poland.Validators;

public class PolandIbanValidator : IIbanValidator
{
    public string CountryCode => "PL";
    private const int PolandIbanLength = 28;
    private const string PolandCountryCode = "PL";

    public ValidationResult Validate(string? iban) => ValidatePolandIban(iban);

    public static ValidationResult ValidatePolandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != PolandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {PolandIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(PolandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected PL.");
        }

        // Structure check: All digits
        for (int i = 2; i < PolandIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Poland IBAN must contain only digits after the country code.");
            }
        }

        // Note: Poland uses an internal Modulo 97 on the BBAN (last 24 digits).
        // The global IBAN validation ensures this mathematically.
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
