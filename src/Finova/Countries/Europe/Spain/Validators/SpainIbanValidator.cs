using System.Diagnostics.CodeAnalysis;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish bank accounts.
/// Format: ES (2) + Check (2) + Entidad (4) + Oficina (4) + DC (2) + Cuenta (10).
/// Total Length: 24
/// </summary>
public class SpainIbanValidator : IIbanValidator
{
    public string CountryCode => "ES";
    private const int SpainIbanLength = 24;
    private const string SpainCountryCode = "ES";

    public bool IsValidIban(string? iban) => ValidateSpainIban(iban);

    public static bool ValidateSpainIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return false;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SpainIbanLength)
        {
            return false;
        }

        if (!normalized.StartsWith(SpainCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }

        for (int i = 2; i < 24; i++)
        {
            if (!char.IsDigit(normalized[i])) return false;
        }

        return IbanHelper.IsValidIban(normalized);
    }
}
