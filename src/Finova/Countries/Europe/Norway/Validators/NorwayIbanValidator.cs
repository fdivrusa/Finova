using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Norway.Validators;

public class NorwayIbanValidator : IIbanValidator
{
    public string CountryCode => "NO";
    private const int NorwayIbanLength = 15;
    private const string NorwayCountryCode = "NO";

    public bool IsValidIban(string? iban)
    {
        return ValidateNorwayIban(iban);
    }

    public static bool ValidateNorwayIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != NorwayIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(NorwayCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All body characters must be digits
        for (int i = 2; i < NorwayIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // Internal Validation: Modulo 11 on BBAN (Last 11 digits)
        // Indices 4 to 15 in IBAN
        string bban = normalized.Substring(4, 11);
        if (!ValidateMod11(bban))
        {
            return false;
        }

        return IbanHelper.IsValidIban(normalized);
    }

    /// <summary>
    /// Validates Norwegian BBAN using Modulo 11 with weights.
    /// Weights sequence: 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 on the first 10 digits.
    /// </summary>
    private static bool ValidateMod11(string bban)
    {
        // BBAN is 11 digits. Last one is check digit.
        int[] weights = [5, 4, 3, 2, 7, 6, 5, 4, 3, 2];
        int sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += (bban[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1)
        {
            return false; // Mod11 "10" exception (invalid account)
        }
        else
        {
            checkDigit = 11 - remainder;
        }

        return checkDigit == (bban[10] - '0');
    }
}
