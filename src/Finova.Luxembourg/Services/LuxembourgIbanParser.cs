using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Luxembourg.Models;
using Finova.Luxembourg.Validators;

namespace Finova.Luxembourg.Services
{
    public class LuxembourgIbanParser(LuxembourgIbanValidator validator) : IIbanParser
    {
        private readonly LuxembourgIbanValidator _validator = validator;

        public string? CountryCode => _validator.CountryCode;

        //static method
        public static LuxembourgIbanParser Create()
        {
            return new LuxembourgIbanParser(new LuxembourgIbanValidator());
        }

        public IbanDetails? ParseIban(string? iban)
        {
            if (!_validator.IsValidIban(iban))
            {
                return null;
            }
            var normalized = IbanHelper.NormalizeIban(iban);

            return new LuxembourgIbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],     // "LU"
                CheckDigits = normalized[2..4],     // Check digits

                // Specific properties
                BankCodeLu = normalized[4..7],  // 3 digits
                AccountNumberLu = normalized[7..20], // 13 alphanumeric characters

                // Generic properties (Mapping)
                BankCode = normalized[4..7],
                AccountNumber = normalized[7..20],

                IsValid = true
            };
        }
    }
}
