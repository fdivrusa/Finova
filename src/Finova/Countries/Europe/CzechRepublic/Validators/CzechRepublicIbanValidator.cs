using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicIbanValidator : IIbanValidator
{
    public string CountryCode => "CZ";
    private const int CzechIbanLength = 24;
    private const string CzechCountryCode = "CZ";

    public ValidationResult Validate(string? iban) => ValidateCzechIban(iban);

    public static ValidationResult ValidateCzechIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CzechIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {CzechIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(CzechCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected CZ.");
        }

        // Structure check: All digits
        for (int i = 2; i < CzechIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Czech IBAN must contain only digits after the country code.");
            }
        }

        // --- Specific CZ Validation (Modulo 11) ---
        // CZ IBAN Structure:
        // Pos 0-3: CZ + Check
        // Pos 4-8: Bank Code (4 digits)
        // Pos 8-14: Prefix (6 digits)
        // Pos 14-24: Account Number (10 digits)

        string prefix = normalized.Substring(8, 6);
        string accountNumber = normalized.Substring(14, 10);

        // Validate Prefix (only if not all zeros)
        if (long.TryParse(prefix, out long prefixVal) && prefixVal > 0)
        {
            // Note: Prefix is 6 digits. We use the last 6 weights: 10, 5, 8, 4, 2, 1
            if (!ValidateCzechMod11(prefix, true))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Czech Prefix checksum.");
            }
        }

        // Validate Account Number
        if (!ValidateCzechMod11(accountNumber, false))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Czech Account Number checksum.");
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    /// <summary>
    /// Validates Czech Prefix or Account Number using Weighted Modulo 11.
    /// Weights: 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 (for 10 digits).
    /// </summary>
    private static bool ValidateCzechMod11(string input, bool isPrefix)
    {
        // Full weights for 10 digits
        int[] weights = [6, 3, 7, 9, 10, 5, 8, 4, 2, 1];

        int sum = 0;
        int inputLength = input.Length;

        // If checking prefix (6 digits), we align to the right of the weights array.
        // Prefix corresponds to the last 6 positions of the weight array.
        int weightStartIndex = isPrefix ? 4 : 0;

        for (int i = 0; i < inputLength; i++)
        {
            int digit = input[i] - '0';
            int weight = weights[weightStartIndex + i];
            sum += digit * weight;
        }

        return sum % 11 == 0;
    }
}
