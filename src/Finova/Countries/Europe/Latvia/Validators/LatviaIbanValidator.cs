using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Latvia.Validators;

public class LatviaIbanValidator : IIbanValidator
{
    public string CountryCode => "LV";
    private const int LatviaIbanLength = 21;
    private const string LatviaCountryCode = "LV";

    public bool IsValidIban(string? iban)
    {
        return ValidateLatviaIban(iban);
    }

    public static bool ValidateLatviaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LatviaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(LatviaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: Alphanumeric
        // Bank Code (4 chars) + Account (13 chars)
        for (int i = 4; i < LatviaIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
