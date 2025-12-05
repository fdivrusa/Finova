using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Vatican.Validators;

public class VaticanIbanValidator : IIbanValidator
{
    public string CountryCode => "VA";
    private const int VaticanIbanLength = 22;
    private const string VaticanCountryCode = "VA";

    public bool IsValidIban(string? iban)
    {
        return ValidateVaticanIban(iban);
    }

    public static bool ValidateVaticanIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != VaticanIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(VaticanCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        for (int i = 2; i < VaticanIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
