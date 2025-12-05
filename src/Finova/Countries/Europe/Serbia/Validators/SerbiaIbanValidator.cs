using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Serbia.Validators;

public class SerbiaIbanValidator : IIbanValidator
{
    public string CountryCode => "RS";
    private const int SerbiaIbanLength = 22;
    private const string SerbiaCountryCode = "RS";

    public bool IsValidIban(string? iban)
    {
        return ValidateSerbiaIban(iban);
    }

    public static bool ValidateSerbiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SerbiaIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(SerbiaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        // Structure check: All digits
        for (int i = 2; i < SerbiaIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return false;
            }
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
