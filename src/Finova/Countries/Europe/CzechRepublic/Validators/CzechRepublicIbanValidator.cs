using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.CzechRepublic.Validators;

public class CzechRepublicIbanValidator : IIbanValidator
{
    public string CountryCode => "CZ";
    private const int CzechIbanLength = 24;
    private const string CzechCountryCode = "CZ";

    public bool IsValidIban(string? iban)
    {
        return ValidateCzechIban(iban);
    }

    public static bool ValidateCzechIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CzechIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(CzechCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        for (int i = 2; i < CzechIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
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
                return false;
            }
        }

        // Validate Account Number
        if (!ValidateCzechMod11(accountNumber, false))
        {
            return false;
        }

        return IbanHelper.IsValidIban(normalized);
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
