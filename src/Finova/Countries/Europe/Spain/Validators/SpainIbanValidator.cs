using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish bank accounts.
/// Format: ES (2) + Check (2) + Entidad (4) + Oficina (4) + DC (2) + Cuenta (10).
/// Total Length: 24.
/// The 'DC' (Dígito de Control) validates the Bank/Branch and the Account separately using Modulo 11.
/// </summary>
public class SpainIbanValidator : IIbanValidator
{
    public string CountryCode => "ES";
    private const int SpainIbanLength = 24;
    private const string SpainCountryCode = "ES";

    // Standard weights for Spanish Modulo 11 algorithm
    private static readonly int[] Weights = [1, 2, 4, 8, 5, 10, 9, 7, 3, 6];

    public bool IsValidIban(string? iban) => ValidateSpainIban(iban);

    public static bool ValidateSpainIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SpainIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(SpainCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        for (int i = 2; i < 24; i++)
        {
            if (!char.IsDigit(normalized[i])) return false;
        }

        // Extract parts:
        // Bank (Entidad): 4 chars (Pos 4-8)
        // Branch (Oficina): 4 chars (Pos 8-12)
        // DC (Check Digits): 2 chars (Pos 12-14)
        // Account (Cuenta): 10 chars (Pos 14-24)

        string bank = normalized.Substring(4, 4);
        string branch = normalized.Substring(8, 4);
        string dc = normalized.Substring(12, 2);
        string account = normalized.Substring(14, 10);

        // Calculate First DC (Validates Bank + Branch)
        // Input must be padded with "00" to make 10 digits: "00" + Bank + Branch
        int calculatedDc1 = CalculateSpanishDigit("00" + bank + branch);

        // Calculate Second DC (Validates Account)
        // Input is just the 10-digit Account
        int calculatedDc2 = CalculateSpanishDigit(account);

        // Verify against the DC present in the IBAN
        // dc[0] is the first digit, dc[1] is the second
        if ((dc[0] - '0') != calculatedDc1 || (dc[1] - '0') != calculatedDc2)
        {
            return false;
        }

        return IbanHelper.IsValidIban(normalized);
    }

    /// <summary>
    /// Calculates a single Spanish Control Digit (Modulo 11).
    /// </summary>
    /// <param name="value">A 10-digit numeric string.</param>
    /// <returns>The calculated check digit (0-9).</returns>
    private static int CalculateSpanishDigit(string value)
    {
        if (value.Length != 10)
        {
            return -1; // Should not happen given logic above
        }

        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            // Character '0' has int value 48. subtracting '0' gives the numeric value.
            int digit = value[i] - '0';
            sum += digit * Weights[i];
        }

        // Algorithm: 11 - (Sum % 11)
        int remainder = 11 - (sum % 11);

        // Special cases:
        // If remainder is 11, digit is 0
        // If remainder is 10, digit is 1
        return remainder switch
        {
            11 => 0,
            10 => 1,
            _ => remainder
        };
    }
}
