using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Germany.Models;
using Finova.Germany.Validators;

namespace Finova.Germany.Services
{
    public class GermanyIbanParser(GermanyIbanValidator validator) : IIbanParser
    {
        private readonly GermanyIbanValidator _validator = validator;
        public string? CountryCode => _validator.CountryCode;

        /// <summary>
        /// Creates a new parser instance with a default validator.
        /// Use this for non-DI scenarios or quick one-off parsing.
        /// </summary>
        /// <remarks>
        /// For ASP.NET Core applications, prefer dependency injection via AddFinovaGermany().
        /// </remarks>
        /// <example>
        /// <code>
        /// var parser = GermanyIbanParser.Create();
        /// var details = parser.ParseIban("DE89370400440532013000");
        /// </code>
        /// </example>
        public static GermanyIbanParser CreateDefault()
        {
            return new GermanyIbanParser(new GermanyIbanValidator());
        }

        public IbanDetails? ParseIban(string? iban)
        {
            if (!_validator.IsValidIban(iban)) return null;

            var normalized = IbanHelper.NormalizeIban(iban);

            return new GermanyIbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],     // "DE"
                CheckDigits = normalized[2..4],     // Check digits

                // Specific properties
                Bankleitzahl = normalized[4..12],   // 8 digits (BLZ)
                Kontonummer = normalized[12..22],   // 10 digits

                // Generic properties (Mapping)
                BankCode = normalized[4..12],       // BLZ maps to BankCode
                AccountNumber = normalized[12..22], // Konto maps to AccountNumber

                // BranchCode is usually null/empty for Germany 
                // as routing is fully handled by BLZ.

                IsValid = true
            };
        }
    }
}
