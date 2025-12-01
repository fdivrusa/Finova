using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Austria.Validators;

public class AustriaIbanValidator : IIbanValidator
{
    public string CountryCode => "AT";
    private const int AustriaIbanLength = 20;

    public bool IsValidIban(string? iban) => ValidateAustriaIban(iban);

    public static bool ValidateAustriaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AustriaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Austria IBANs are strictly numeric after the country code.
        // Positions 2 to 19 (Check digits + BLZ + Kontonummer)
        for (int i = 2; i < AustriaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
