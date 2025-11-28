using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Spain.Validators
{
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

            // Check length (Spain is 24 chars)
            if (normalized.Length != SpainIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(SpainCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Spain IBANs (after the country code) are strictly numeric.
            // Entidad, Oficina, DC, and Cuenta must all be digits.
            for (int i = 2; i < 24; i++)
            {
                if (!char.IsDigit(normalized[i])) return false;
            }

            // Standard Mod-97 check
            return IbanHelper.IsValidIban(normalized);
        }
    }
}