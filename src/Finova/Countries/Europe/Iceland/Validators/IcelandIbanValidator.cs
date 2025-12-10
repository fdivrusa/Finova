using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Iceland.Validators;

public class IcelandIbanValidator : IIbanValidator
{
    public string CountryCode => "IS";
    private const int IcelandIbanLength = 26;
    private const string IcelandCountryCode = "IS";

    public ValidationResult Validate(string? iban) => ValidateIcelandIban(iban);

    public static ValidationResult ValidateIcelandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != IcelandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {IcelandIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(IcelandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected IS.");
        }

        // Structure check: All digits
        for (int i = 2; i < IcelandIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Iceland IBAN must contain only digits after the country code.");
            }
        }

        // Internal Validation: Kennitala (National ID)
        // The last 10 digits (Pos 16-26) are the Kennitala.
        string kennitala = normalized.Substring(16, 10);
        if (!ValidateKennitala(kennitala))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Icelandic Kennitala.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates the Icelandic National ID (Kennitala).
    /// Uses Modulo 11 with weights: 3, 2, 7, 6, 5, 4, 3, 2.
    /// The 9th digit is the check digit.
    /// </summary>
    private static bool ValidateKennitala(string kt)
    {
        // Kennitala weights for the first 8 digits
        int[] weights = [3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (kt[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder == 0 ? 0 : 11 - remainder;

        if (checkDigit == 10)
        {
            return false; // Invalid Kennitala
        }

        // The 9th digit (index 8) is the control digit
        return checkDigit == (kt[8] - '0');
    }
}
