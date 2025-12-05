using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

/// <summary>
/// Validator for North Macedonia IBANs.
/// </summary>
public class NorthMacedoniaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for North Macedonia.
    /// </summary>
    public string CountryCode => "MK";

    private const int NorthMacedoniaIbanLength = 19;
    private const string NorthMacedoniaCountryCode = "MK";

    /// <summary>
    /// Validates the North Macedonia IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public bool IsValidIban(string? iban)
    {
        return ValidateNorthMacedoniaIban(iban);
    }

    /// <summary>
    /// Static validation method for North Macedonia IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static bool ValidateNorthMacedoniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != NorthMacedoniaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(NorthMacedoniaCountryCode, StringComparison.OrdinalIgnoreCase))
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

        // 2. Account Number (Pos 7-17): 10 alphanumeric characters
        for (int i = 7; i < 17; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        // 3. National Check Digits (Pos 17-19): 2 digits
        for (int i = 17; i < 19; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
