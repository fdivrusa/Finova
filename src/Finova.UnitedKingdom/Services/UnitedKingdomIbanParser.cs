using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.UnitedKingdom.Models;
using Finova.UnitedKingdom.Validators;

namespace Finova.UnitedKingdom.Services
{
    public class UnitedKingdomIbanParser(UnitedKingdomIbanValidator validator) : IIbanParser
    {
        private readonly UnitedKingdomIbanValidator _validator = validator;

        public string? CountryCode => _validator.CountryCode;

        /// <summary>
        /// Creates a new parser instance with a default validator.
        /// Use this for non-DI scenarios or quick one-off parsing.
        /// </summary>
        /// <remarks>
        /// For ASP.NET Core applications, prefer dependency injection via AddFinovaUnitedKingdom().
        /// </remarks>
        /// <example>
        /// <code>
        /// var parser = UnitedKingdomIbanParser.Create();
        /// var details = parser.ParseIban("GB29MIDL40051512345678");
        /// </code>
        /// </example>
        public static UnitedKingdomIbanParser Create()
        {
            return new UnitedKingdomIbanParser(new UnitedKingdomIbanValidator());
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

            return new UnitedKingdomIbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],
                CheckDigits = normalized[2..4],

                BankCode = normalized[4..8],
                BankIdentifier = normalized[4..8],

                BranchCode = normalized[8..14],
                SortCode = normalized[8..14],

                AccountNumber = normalized[14..22],
                UkAccountNumber = normalized[14..22],

                NationalCheckKey = null,
                IsValid = true
            };
        }
    }
}
