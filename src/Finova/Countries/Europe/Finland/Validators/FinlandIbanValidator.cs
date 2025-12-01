using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Finland.Validators;

public class FinlandIbanValidator : IIbanValidator
{
    public string CountryCode => "FI";
    private const int FinlandIbanLength = 18;

    public bool IsValidIban(string? iban) => ValidateFinlandIban(iban);

    public static bool ValidateFinlandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != FinlandIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith("FI", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Finland IBANs are strictly numeric after the country code.
        for (int i = 2; i < FinlandIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
