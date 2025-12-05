using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Estonia.Validators;

public class EstoniaIbanValidator : IIbanValidator
{
    public string CountryCode => "EE";
    private const int EstoniaIbanLength = 20;
    private const string EstoniaCountryCode = "EE";

    public bool IsValidIban(string? iban)
    {
        return ValidateEstoniaIban(iban);
    }

    public static bool ValidateEstoniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != EstoniaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(EstoniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        for (int i = 2; i < EstoniaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // Internal Validation: 7-3-1 Method on BBAN
        // EE BBAN is the last 16 digits (Pos 4-20)
        // This validates the check digit which is the last character of the BBAN.
        string bban = normalized.Substring(4, 16);
        if (!ValidateEstonian731(bban))
        {
            return false;
        }

        return IbanHelper.IsValidIban(normalized);
    }

    private static bool ValidateEstonian731(string bban)
    {
        // Weights: 7, 3, 1 repeating
        int[] weights = [7, 3, 1];
        int sum = 0;

        // Iterate over the first 15 digits (last one is check digit)
        for (int i = 0; i < 15; i++)
        {
            int digit = bban[i] - '0';
            // Determine weight based on position (cycling 0, 1, 2)
            int weight = weights[i % 3];
            sum += digit * weight;
        }

        // Calculate Check Digit
        // Formula: (10 - (Sum % 10)) % 10
        int remainder = sum % 10;
        int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

        return checkDigit == (bban[15] - '0');
    }
}
