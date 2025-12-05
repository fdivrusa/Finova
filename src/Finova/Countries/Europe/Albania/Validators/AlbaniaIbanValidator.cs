using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian IBANs.
/// </summary>
public class AlbaniaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Albania.
    /// </summary>
    public string CountryCode => "AL";

    private const int AlbaniaIbanLength = 28;
    private const string AlbaniaCountryCode = "AL";

    /// <summary>
    /// Validates the Albanian IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public bool IsValidIban(string? iban)
    {
        return ValidateAlbaniaIban(iban);
    }

    /// <summary>
    /// Static validation method for Albanian IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static bool ValidateAlbaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AlbaniaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(AlbaniaCountryCode, StringComparison.OrdinalIgnoreCase))
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

        // 2. Branch Code (Pos 7-11): 4 digits
        for (int i = 7; i < 11; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 3. Control Character (Pos 11): 1 alphanumeric character (usually check digit for BBAN, but part of IBAN structure)
        if (!char.IsLetterOrDigit(normalized[11]))
        {
            return false;
        }

        // 4. Account Number (Pos 12-28): 16 alphanumeric characters
        for (int i = 12; i < 28; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
