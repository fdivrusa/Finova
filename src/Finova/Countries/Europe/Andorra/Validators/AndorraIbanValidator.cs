using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Andorra.Validators;

public class AndorraIbanValidator : IIbanValidator
{
    public string CountryCode => "AD";
    private const int AndorraIbanLength = 24;
    private const string AndorraCountryCode = "AD";

    public bool IsValidIban(string? iban)
    {
        return ValidateAndorraIban(iban);
    }

    public static bool ValidateAndorraIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AndorraIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(AndorraCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check:
        // Bank (4) and Branch (4) are typically numeric or alphanumeric?
        // Standard allows alphanumeric for Andorra account part, usually numeric for bank/branch.
        // Let's enforce Alphanumeric globally for safety as per generic registry.
        for (int i = 4; i < AndorraIbanLength; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
