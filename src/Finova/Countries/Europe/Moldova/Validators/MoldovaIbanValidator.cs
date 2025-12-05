using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldovan IBANs.
/// </summary>
public class MoldovaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Moldova.
    /// </summary>
    public string CountryCode => "MD";

    private const int MoldovaIbanLength = 24;
    private const string MoldovaCountryCode = "MD";

    /// <summary>
    /// Validates the Moldovan IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public bool IsValidIban(string? iban)
    {
        return ValidateMoldovaIban(iban);
    }

    /// <summary>
    /// Static validation method for Moldovan IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static bool ValidateMoldovaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MoldovaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(MoldovaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-6): 2 alphanumeric characters
        for (int i = 4; i < 6; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        // 2. Account Number (Pos 6-24): 18 alphanumeric characters
        for (int i = 6; i < 24; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
