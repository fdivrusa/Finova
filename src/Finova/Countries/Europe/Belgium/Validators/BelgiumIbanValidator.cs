using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian IBAN bank accounts.
/// Belgian IBAN format: BE + 2 check digits + 3 Bank + 7 Account + 2 National Check.
/// Total: 16 characters.
/// </summary>
public class BelgiumIbanValidator : IIbanValidator
{
    public string CountryCode => "BE";

    private const int BelgianIbanLength = 16;
    private const string BelgianCountryCode = "BE";

    public ValidationResult Validate(string? iban) => ValidateBelgiumIban(iban);

    public static ValidationResult ValidateBelgiumIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BelgianIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {BelgianIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(BelgianCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected BE.");
        }

        for (int i = 2; i < 16; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Belgium IBAN must contain only digits after the country code.");
            }
        }

        // Internal BBAN Validation (Specific to Belgium)
        // Extract the last 12 digits (Positions 4 to 16)
        string bban = normalized.Substring(4, 12);

        if (!ValidateBelgianBban(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Belgian BBAN structure.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates the internal Belgian BBAN structure.
    /// Format: 10 digits (Bank + Account) + 2 digits (Check).
    /// Algorithm: First10 % 97 == Last2.
    /// </summary>
    private static bool ValidateBelgianBban(string bban)
    {
        if (bban.Length != 12)
        {
            return false;
        }

        // Split into Data (10 digits) and Check (2 digits)
        string dataPart = bban.Substring(0, 10);
        string checkPart = bban.Substring(10, 2);

        if (!long.TryParse(dataPart, out long dataValue) ||
            !int.TryParse(checkPart, out int checkValue))
        {
            return false;
        }

        // Calculate Modulo
        long remainder = dataValue % 97;

        // Specific rule: If remainder is 0, the check digits must be 97
        if (remainder == 0)
        {
            return checkValue == 97;
        }

        return remainder == checkValue;
    }
}
