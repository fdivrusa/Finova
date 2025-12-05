using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Cyprus.Validators;

public class CyprusIbanValidator : IIbanValidator
{
    public string CountryCode => "CY";
    private const int CyprusIbanLength = 28;
    private const string CyprusCountryCode = "CY";

    public bool IsValidIban(string? iban)
    {
        return ValidateCyprusIban(iban);
    }

    public static bool ValidateCyprusIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CyprusIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(CyprusCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:

        // 1. Bank Code (Pos 4-7): Must be digits
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 2. Branch Code (Pos 7-12): Must be digits
        for (int i = 7; i < 12; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 3. Account Number (Pos 12-28): Alphanumeric
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
