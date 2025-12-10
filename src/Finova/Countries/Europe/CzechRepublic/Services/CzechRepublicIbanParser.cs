using Finova.Core.Iban;


using Finova.Countries.Europe.CzechRepublic.Models;
using Finova.Countries.Europe.CzechRepublic.Validators;

namespace Finova.Countries.Europe.CzechRepublic.Services;

public class CzechRepublicIbanParser(CzechRepublicIbanValidator validator) : IIbanParser
{
    public static CzechRepublicIbanParser Create() => new(new CzechRepublicIbanValidator());
    private readonly CzechRepublicIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new CzechRepublicIbanDetails
        {
            Iban = normalized,
            CountryCode = "CZ",
            CheckDigits = normalized[2..4],
            KodBanky = normalized[4..8],
            Predcisli = normalized[8..14],
            CisloUctu = normalized[14..24],

            // Generic Mapping
            BankCode = normalized[4..8],
            BranchCode = normalized[8..14], // Mapping Prefix to BranchCode for generic use
            AccountNumber = normalized[14..24],
            IsValid = true
        };
    }
}
