using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Irish bank accounts.
/// Ireland IBAN format: IE + 2 check + 4 Bank Code + 6 Sort Code + 8 Account (22 characters total).
/// </summary>
public class IrelandIbanValidator : IIbanValidator
{
    public string CountryCode => "IE";

    private const int IrelandIbanLength = 22;
    private const string IrelandCountryCode = "IE";

    #region Instance Methods (for Dependency Injection)

    public bool IsValidIban(string? iban)
    {
        return ValidateIrelandIban(iban);
    }

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates an Irish IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    public static bool ValidateIrelandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Check length (Ireland IBAN is exactly 22 characters)
        if (normalized.Length != IrelandIbanLength)
        {
            return false;
        }

        // Check country code
        if (!normalized.StartsWith(IrelandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:

        // Bank Code / BIC (Pos 4-7) must be letters
        // Example: "AIBK"
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return false;
            }
        }

        // Sort Code (Pos 8-13) and Account Number (Pos 14-21) must be digits
        // Total range from index 8 to 21
        for (int i = 8; i < 22; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }

    #endregion
}
