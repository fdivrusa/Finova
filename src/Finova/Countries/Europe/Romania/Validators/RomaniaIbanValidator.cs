using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Romania.Validators;

public class RomaniaIbanValidator : IIbanValidator
{
    public string CountryCode => "RO";
    private const int RomaniaIbanLength = 24;
    private const string RomaniaCountryCode = "RO";

    public bool IsValidIban(string? iban)
    {
        return ValidateRomaniaIban(iban);
    }

    public static bool ValidateRomaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != RomaniaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(RomaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: Alphanumeric
        // Bank Code (Pos 4-8) and Account Number (Pos 8-24) can contain letters.
        for (int i = 4; i < RomaniaIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
