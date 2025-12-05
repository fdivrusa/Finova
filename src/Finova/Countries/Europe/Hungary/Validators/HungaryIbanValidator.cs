using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Hungary.Validators;

public class HungaryIbanValidator : IIbanValidator
{
    public string CountryCode => "HU";
    private const int HungaryIbanLength = 28;
    private const string HungaryCountryCode = "HU";

    public bool IsValidIban(string? iban)
    {
        return ValidateHungaryIban(iban);
    }

    public static bool ValidateHungaryIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != HungaryIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(HungaryCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All body characters must be digits
        for (int i = 2; i < HungaryIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
