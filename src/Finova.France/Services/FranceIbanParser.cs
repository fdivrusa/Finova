using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.France.Models;
using Finova.France.Validators;

namespace Finova.France.Services
{
    public class FranceIbanParser(FranceIbanValidator validator) : IIbanParser
    {
        private readonly FranceIbanValidator _validator = validator;

        public string? CountryCode => _validator.CountryCode;

        /// <summary>
        /// Creates a new parser instance with a default validator.
        /// Use this for non-DI scenarios or quick one-off parsing.
        /// </summary>
        /// <remarks>
        /// For ASP.NET Core applications, prefer dependency injection via AddFinovaFrance().
        /// </remarks>
        /// <example>
        /// <code>
        /// var parser = FranceIbanParser.Create();
        /// var details = parser.ParseIban("FR1420041010050500013M02606");
        /// </code>
        /// </example>
        public static FranceIbanParser Create()
        {
            return new FranceIbanParser(new FranceIbanValidator());
        }

        public IbanDetails? ParseIban(string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return null;
            }

            if (!_validator.IsValidIban(iban))
            {
                return null;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            return new FranceIbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],     // "FR"
                CheckDigits = normalized[2..4],     // Check digits

                // Specific properties
                BankCodeFr = normalized[4..9],      // 5 digits
                BranchCodeFr = normalized[9..14],   // 5 digits
                AccountNumberFr = normalized[14..25], // 11 chars
                RibKey = normalized[25..27],        // 2 digits

                // Generic properties (Mapping)
                BankCode = normalized[4..9],
                BranchCode = normalized[9..14],     // France uses Branch/Guichet
                AccountNumber = normalized[14..25],
                NationalCheckKey = normalized[25..27], // France has a specific National Key

                IsValid = true
            };
        }
    }
}
