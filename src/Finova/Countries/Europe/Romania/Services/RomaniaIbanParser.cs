using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Romania.Models;
using Finova.Countries.Europe.Romania.Validators;

namespace Finova.Countries.Europe.Romania.Services;

public class RomaniaIbanParser(RomaniaIbanValidator validator) : IIbanParser
{
    public static RomaniaIbanParser Create() => new(new RomaniaIbanValidator());
    private readonly RomaniaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new RomaniaIbanDetails
        {
            Iban = normalized,
            CountryCode = "RO",
            CheckDigits = normalized[2..4],
            CodulBancii = normalized[4..8],
            NumarCont = normalized[8..24],

            // Generic Mapping
            BankCode = normalized[4..8],
            AccountNumber = normalized[8..24],
            IsValid = true
        };
    }
}
