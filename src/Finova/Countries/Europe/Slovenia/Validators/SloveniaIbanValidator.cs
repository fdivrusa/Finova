using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Slovenia.Validators;

public class SloveniaIbanValidator : IIbanValidator
{
    public string CountryCode => "SI";
    private const int SloveniaIbanLength = 19;
    private const string SloveniaCountryCode = "SI";

    public bool IsValidIban(string? iban)
    {
        return ValidateSloveniaIban(iban);
    }

    public static bool ValidateSloveniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SloveniaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(SloveniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: Digits only
        for (int i = 2; i < SloveniaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        // Note: Slovenia uses Mod 97 for the internal BBAN (last 15 digits).
        // Since the IBAN uses Mod 97 globally, strictly validating the IBAN envelope 
        // covers the internal integrity.

        return IbanHelper.IsValidIban(normalized);
    }
}
