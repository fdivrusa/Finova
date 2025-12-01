using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for Dutch (Netherlands) IBAN bank accounts.
/// Dutch IBAN format: NL + 2 check digits + 4 bank code + 10 account number (18 characters total).
/// Example: NL91ABNA0417164300.
/// </summary>
public class NetherlandsIbanValidator : IIbanValidator
{
    public string CountryCode => "NL";

    private const int DutchIbanLength = 18;
    private const string DutchCountryCode = "NL";

    #region Instance Methods (for Dependency Injection)

    public bool IsValidIban(string? iban)
    {
        return ValidateNetherlandsIban(iban);
    }

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates a Dutch IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate</param>
    /// <returns>True if valid Dutch IBAN, false otherwise</returns>
    public static bool ValidateNetherlandsIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != DutchIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(DutchCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Bank Code: Positions 4-7 (4 chars) must be letters (e.g., ABNA, INGB, RABO)
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i])) return false;
        }

        // Account Number: Positions 8-17 (10 chars) must be digits
        for (int i = 8; i < 18; i++)
        {
            if (!char.IsDigit(normalized[i])) return false;
        }

        // Verify the consistency of the 10-digit account number
        string accountNumber = normalized.Substring(8, 10);
        if (!ValidateElfProef(accountNumber))
        {
            return false;
        }

        return IbanHelper.IsValidIban(normalized);
    }

    /// <summary>
    /// Validates the Dutch "Elfproef" (11-test) on the 10-digit account number.
    /// Algorithm: Sum of (Digit * Weight) must be divisible by 11.
    /// Weights are: 10, 9, 8, 7, 6, 5, 4, 3, 2, 1.
    /// </summary>
    private static bool ValidateElfProef(string account)
    {
        int sum = 0;

        // Iterate over the 10 digits
        for (int i = 0; i < 10; i++)
        {
            int digit = account[i] - '0';

            // Weight decreases from 10 down to 1
            // Index 0 -> Weight 10
            // Index 1 -> Weight 9
            // ...
            // Index 9 -> Weight 1
            int weight = 10 - i;

            sum += digit * weight;
        }

        return sum % 11 == 0;
    }

    #endregion
}
