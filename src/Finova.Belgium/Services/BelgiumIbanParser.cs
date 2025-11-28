using Finova.Belgium.Models;
using Finova.Belgium.Validators;
using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;

namespace Finova.Belgium.Services
{
    public class BelgiumIbanParser(BelgiumIbanValidator validator) : IIbanParser
    {
        private readonly BelgiumIbanValidator _validator = validator;
        public string? CountryCode => _validator.CountryCode;

        /// <summary>
        /// Creates a new parser instance with a default validator.
        /// Use this for non-DI scenarios or quick one-off parsing.
        /// </summary>
        /// <remarks>
        /// For ASP.NET Core applications, prefer dependency injection via AddFinovaBelgium().
        /// </remarks>
        /// <example>
        /// <code>
        /// var parser = BelgiumIbanParser.Create();
        /// var details = parser.ParseIban("BE685 39007547034");
        /// </code>
        /// </example>
        public static BelgiumIbanParser Create()
        {
            return new BelgiumIbanParser(new BelgiumIbanValidator());
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

            return new BelgiumIbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],     // "BE"
                CheckDigits = normalized[2..4],     // Check digits

                // Specific properties
                BankCodeBe = normalized[4..7],      // 3 digits
                AccountNumberBe = normalized[7..14], // 7 digits
                BelgianCheckKey = normalized[14..16], // 2 digits (Validation Key)

                // Generic properties (Mapping)
                BankCode = normalized[4..7],
                AccountNumber = normalized[7..14],  // Mapping the core account part
                NationalCheckKey = normalized[14..16], // Maps nicely to NationalKey

                IsValid = true
            };
        }
    }
}
