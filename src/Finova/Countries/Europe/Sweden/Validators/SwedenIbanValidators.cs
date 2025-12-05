using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Sweden.Validators;

public class SwedenIbanValidator : IIbanValidator
{
    public string CountryCode => "SE";
    private const int SwedenIbanLength = 24;
    private const string SwedenCountryCode = "SE";

    public bool IsValidIban(string? iban)
    {
        return ValidateSwedenIban(iban);
    }

    public static bool ValidateSwedenIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SwedenIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(SwedenCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        // Sweden IBAN body (positions 4 to 24) must be numeric
        for (int i = 2; i < SwedenIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
