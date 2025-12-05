using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Poland.Validators;

public class PolandIbanValidator : IIbanValidator
{
    public string CountryCode => "PL";
    private const int PolandIbanLength = 28;
    private const string PolandCountryCode = "PL";

    public bool IsValidIban(string? iban)
    {
        return ValidatePolandIban(iban);
    }

    public static bool ValidatePolandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != PolandIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(PolandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        for (int i = 2; i < PolandIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // Note: Poland uses an internal Modulo 97 on the BBAN (last 24 digits).
        // The global IBAN validation ensures this mathematically.
        return IbanHelper.IsValidIban(normalized);
    }
}
