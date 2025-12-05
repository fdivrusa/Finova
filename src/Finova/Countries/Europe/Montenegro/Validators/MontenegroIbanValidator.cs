using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegro IBANs.
/// </summary>
public class MontenegroIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Montenegro.
    /// </summary>
    public string CountryCode => "ME";

    private const int MontenegroIbanLength = 22;
    private const string MontenegroCountryCode = "ME";

    /// <summary>
    /// Validates the Montenegro IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public bool IsValidIban(string? iban)
    {
        return ValidateMontenegroIban(iban);
    }

    /// <summary>
    /// Static validation method for Montenegro IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static bool ValidateMontenegroIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MontenegroIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(MontenegroCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-7): 3 digits
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 2. Account Number (Pos 7-20): 13 digits
        for (int i = 7; i < 20; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 3. National Check Digits (Pos 20-22): 2 digits
        for (int i = 20; i < 22; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
