using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Croatia.Validators;

public class CroatiaIbanValidator : IIbanValidator
{
    public string CountryCode => "HR";
    private const int CroatiaIbanLength = 21;
    private const string CroatiaCountryCode = "HR";

    public bool IsValidIban(string? iban)
    {
        return ValidateCroatiaIban(iban);
    }

    public static bool ValidateCroatiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CroatiaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(CroatiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: Digits only
        for (int i = 2; i < CroatiaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
