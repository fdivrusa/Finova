using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Portugal.Models;
using Finova.Countries.Europe.Portugal.Validators;

namespace Finova.Countries.Europe.Portugal.Services;

public class PortugalIbanParser(PortugalIbanValidator validator) : IIbanParser
{
    public static PortugalIbanParser Create() => new(new PortugalIbanValidator());
    private readonly PortugalIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic for Portugal:
        // 0-2: Country (PT)
        // 2-4: Check Digits
        // 4-8: Codigo de Banco (4 digits)
        // 8-12: Codigo de Balcao (4 digits)
        // 12-23: Numero de Conta (11 digits)
        // 23-25: Algarismos de Controlo (2 digits)

        return new PortugalIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],
            CheckDigits = normalized[2..4],

            // Localized Fields
            CodigoBanco = normalized[4..8],
            CodigoBalcao = normalized[8..12],
            NumeroConta = normalized[12..23],
            AlgarismosControlo = normalized[23..25],

            // Standard Mapping
            BankCode = normalized[4..8],       // Maps to CodigoBanco
            BranchCode = normalized[8..12],    // Maps to CodigoBalcao
            AccountNumber = normalized[12..23], // Maps to NumeroConta
            NationalCheckKey = normalized[23..25], // Maps to AlgarismosControlo
            IsValid = true
        };
    }
}
