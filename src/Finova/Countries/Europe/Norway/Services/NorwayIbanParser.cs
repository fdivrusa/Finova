using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Norway.Models;
using Finova.Countries.Europe.Norway.Validators;

namespace Finova.Countries.Europe.Norway.Services;

public class NorwayIbanParser(NorwayIbanValidator validator) : IIbanParser
{
    public static NorwayIbanParser Create() => new(new NorwayIbanValidator());
    private readonly NorwayIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new NorwayIbanDetails
        {
            Iban = normalized,
            CountryCode = "NO",
            CheckDigits = normalized[2..4],
            Bankkod = normalized[4..8],
            Kontonummer = normalized[8..14],
            Kontrollsiffer = normalized[14..15],

            // Generic Mapping
            BankCode = normalized[4..8],
            AccountNumber = normalized[8..14],
            NationalCheckKey = normalized[14..15],
            IsValid = true
        };
    }
}
