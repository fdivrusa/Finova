using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Core.Services
{
    public class IbanService : IIbanService
    {

        public string? CountryCode => null;

        public bool IsValidIban([NotNullWhen(true)] string? iban)
        {
            return IbanHelper.IsValidIban(iban);
        }

        public string FormatIban(string? iban)
        {
            return IbanHelper.FormatIban(iban);
        }

        public string NormalizeIban(string? iban)
        {
            return IbanHelper.NormalizeIban(iban);
        }

        public string GetCountryCode(string? iban)
        {
            return IbanHelper.GetCountryCode(iban);
        }

        public int GetCheckDigits(string? iban)
        {
            return IbanHelper.GetCheckDigits(iban);
        }

        public bool ValidateChecksum([NotNullWhen(true)] string? iban)
        {
            return IbanHelper.ValidateChecksum(iban);
        }
    }
}
