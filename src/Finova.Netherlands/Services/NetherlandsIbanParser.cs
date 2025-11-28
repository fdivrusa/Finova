using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Netherlands.Models;
using Finova.Netherlands.Validators;

namespace Finova.Netherlands.Services
{
    public class NetherlandsIbanParser(NetherlandsIbanValidator validator) : IIbanParser
    {
        private readonly NetherlandsIbanValidator _validator = validator;

        public string? CountryCode => _validator.CountryCode;

        /// <summary>
        /// Creates a new parser instance with a default validator.
        /// Use this for non-DI scenarios or quick one-off parsing.
        /// </summary>
        /// <remarks>
        /// For ASP.NET Core applications, prefer dependency injection via AddFinovaNetherlands().
        /// </remarks>
        /// <example>
        /// <code>
        /// var parser = NetherlandsIbanParser.Create();
        /// var details = parser.ParseIban("NL91ABNA0417164300");
        /// </code>
        /// </example>
        public static NetherlandsIbanParser Create()
        {
            return new NetherlandsIbanParser(new NetherlandsIbanValidator());
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
            return new NetherlandsIbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],     // "NL"
                CheckDigits = normalized[2..4],     // "91"
                BankCodeNl = normalized[4..8],      // "ABNA"
                AccountNumberNl = normalized[8..18], // "0417164300",
                AccountNumber = normalized[8..18],
                BankCode = normalized[4..8],
                BranchCode = null,
                NationalCheckKey = null,
                IsValid = true
            };
        }
    }
}
