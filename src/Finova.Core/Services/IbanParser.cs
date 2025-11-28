using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;

namespace Finova.Core.Services
{
    public class IbanParser : IIbanParser
    {
        public string? CountryCode => null;  // Generic service

        /// <summary>
        /// Parses an IBAN into generic details (no country-specific parsing).
        /// For country-specific details, use country-specific service implementations.
        /// </summary>
        public static IbanDetails? Parse(string? iban)
        {
            if (!IbanHelper.IsValidIban(iban))
            {
                return null;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            return new IbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[..2],
                CheckDigits = normalized.Substring(2, 2),
                IsValid = true
            };
        }

        /// <summary>
        /// Parses an IBAN into generic details (no country-specific parsing).
        /// </summary>
        /// <param name="iban">The IBAN to parse.</param>
        /// <returns>The parsed IBAN details, or null if invalid.</returns>

        public IbanDetails? ParseIban(string? iban)
        {
            return Parse(iban);
        }
    }
}
