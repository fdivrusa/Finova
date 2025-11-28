using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Spain.Models;
using Finova.Spain.Validators;

namespace Finova.Spain.Services
{
    public class SpainIbanParser(SpainIbanValidator validator) : IIbanParser
    {
        private readonly SpainIbanValidator _validator = validator;
        public string? CountryCode => _validator.CountryCode;

        public static SpainIbanParser Create()
        {
            return new SpainIbanParser(new SpainIbanValidator());
        }

        public IbanDetails? ParseIban(string? iban)
        {
            if (!_validator.IsValidIban(iban))
            {
                return null;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            // Parsing logic based on Spanish Offsets:
            // 0-2:   ES
            // 2-4:   Check Digits
            // 4-8:   Entidad (4 digits)
            // 8-12:  Oficina (4 digits)
            // 12-14: DC (2 digits)
            // 14-24: Cuenta (10 digits)

            string entidad = normalized[4..8];
            string oficina = normalized[8..12];
            string dc = normalized[12..14];
            string cuenta = normalized[14..24];

            return new SpainIbanDetails
            {
                // Base properties
                Iban = normalized,
                CountryCode = normalized[0..2],
                CheckDigits = normalized[2..4],

                // Mapping Spanish concepts to Base Generic concepts
                BankCode = entidad,      // Entidad maps to BankCode
                BranchCode = oficina,    // Oficina maps to BranchCode
                NationalCheckKey = dc,   // DC maps to NationalCheckKey
                AccountNumber = cuenta,  // Cuenta maps to AccountNumber

                // Specific properties
                Entidad = entidad,
                Oficina = oficina,
                DC = dc,
                Cuenta = cuenta,

                IsValid = true
            };
        }
    }
}