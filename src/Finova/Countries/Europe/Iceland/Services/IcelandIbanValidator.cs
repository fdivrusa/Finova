using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Iceland.Models;
using Finova.Countries.Europe.Iceland.Validators;

namespace Finova.Countries.Europe.Iceland.Services;

public class IcelandIbanParser(IcelandIbanValidator validator) : IIbanParser
{
    public static IcelandIbanParser Create() => new(new IcelandIbanValidator());
    private readonly IcelandIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new IcelandIbanDetails
        {
            Iban = normalized,
            CountryCode = "IS",
            CheckDigits = normalized[2..4],
            Banki = normalized[4..8],
            Hb = normalized[8..10],
            Reikningsnumer = normalized[10..16],
            Kennitala = normalized[16..26],

            // Generic Mapping
            BankCode = normalized[4..8],
            BranchCode = normalized[8..10],
            AccountNumber = normalized[10..16], // Account part
            // Note: Kennitala is technically the "owner" identifier, sometimes mapped to AccountNumber in generic systems
            IsValid = true
        };
    }
}
