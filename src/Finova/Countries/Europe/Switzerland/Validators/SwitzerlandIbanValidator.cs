using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Switzerland.Validators;

public class SwitzerlandIbanValidator : IIbanValidator
{
    public string CountryCode => "CH";
    private const int SwitzerlandIbanLength = 21;
    private const string SwitzerlandCountryCode = "CH";

    public bool IsValidIban(string? iban)
    {
        return ValidateSwitzerlandIban(iban);
    }

    public static bool ValidateSwitzerlandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SwitzerlandIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(SwitzerlandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure Validation:

        // 1. Clearing Number (Pos 4-9): Must be digits
        for (int i = 4; i < 9; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // 2. Account Number (Pos 9-21): Alphanumeric
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
