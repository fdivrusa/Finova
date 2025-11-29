using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for France bank accounts.
/// France IBAN format: FR + 2 check digits + 5 bank code + 5 branch code + 11 account number + 2 RIB key (27 characters total).
/// Example: FR1420041010050500013M02606 or formatted: FR14 2004 1010 0505 0001 3M02 606
/// </summary>
public class FranceIbanValidator : IIbanValidator
{
    public string CountryCode => "FR";

    private const int FranceIbanLength = 27;
    private const string FranceCountryCode = "FR";

    public bool IsValidIban([NotNullWhen(true)] string? iban)
    {
        return ValidateFranceIban(iban);
    }

    public static bool ValidateFranceIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != FranceIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(FranceCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        for (int i = 4; i < 14; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        for (int i = 25; i < 27; i++)
        {
            if (!char.IsDigit(normalized[i])) return false;
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
