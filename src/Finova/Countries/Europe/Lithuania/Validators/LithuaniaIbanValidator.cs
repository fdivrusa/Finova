using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Lithuania.Validators;

public class LithuaniaIbanValidator : IIbanValidator
{
    public string CountryCode => "LT";
    private const int LithuaniaIbanLength = 20;
    private const string LithuaniaCountryCode = "LT";

    public bool IsValidIban(string? iban)
    {
        return ValidateLithuaniaIban(iban);
    }

    public static bool ValidateLithuaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LithuaniaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(LithuaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: Digits only
        for (int i = 2; i < LithuaniaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
