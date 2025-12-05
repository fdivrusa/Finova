using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Denmark.Validators;

public class DenmarkIbanValidator : IIbanValidator
{
    public string CountryCode => "DK";
    private const int DenmarkIbanLength = 18;
    private const string DenmarkCountryCode = "DK";

    public bool IsValidIban(string? iban)
    {
        return ValidateDenmarkIban(iban);
    }

    public static bool ValidateDenmarkIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != DenmarkIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(DenmarkCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        for (int i = 2; i < DenmarkIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
