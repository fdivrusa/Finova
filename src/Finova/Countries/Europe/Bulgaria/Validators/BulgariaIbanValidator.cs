using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Bulgaria.Validators;

public class BulgariaIbanValidator : IIbanValidator
{
    public string CountryCode => "BG";
    private const int BulgariaIbanLength = 22;
    private const string BulgariaCountryCode = "BG";

    public bool IsValidIban(string? iban)
    {
        return ValidateBulgariaIban(iban);
    }

    public static bool ValidateBulgariaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != BulgariaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(BulgariaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: Alphanumeric (Bank ID is letters, Account can be alphanumeric)
        for (int i = 4; i < BulgariaIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        // Specific check: Bank Code (Pos 4-8) must be letters
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
