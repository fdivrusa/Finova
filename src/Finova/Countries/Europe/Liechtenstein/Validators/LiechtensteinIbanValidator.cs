using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein IBANs.
/// </summary>
public class LiechtensteinIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Liechtenstein.
    /// </summary>
    public string CountryCode => "LI";

    private const int LiechtensteinIbanLength = 21;
    private const string LiechtensteinCountryCode = "LI";

    /// <summary>
    /// Validates the Liechtenstein IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public bool IsValidIban(string? iban)
    {
        return ValidateLiechtensteinIban(iban);
    }

    /// <summary>
    /// Static validation method for Liechtenstein IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static bool ValidateLiechtensteinIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LiechtensteinIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(LiechtensteinCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-9): 5 digits
        for (int i = 4; i < 9; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 2. Account Number (Pos 9-21): 12 alphanumeric characters
        for (int i = 9; i < 21; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
