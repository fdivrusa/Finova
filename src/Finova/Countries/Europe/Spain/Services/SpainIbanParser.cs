using Finova.Core.Iban;


using Finova.Countries.Europe.Spain.Models;
using Finova.Countries.Europe.Spain.Validators;

namespace Finova.Countries.Europe.Spain.Services;

public class SpainIbanParser(SpainIbanValidator validator) : IIbanParser
{
    private readonly SpainIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public static SpainIbanParser Create() => new SpainIbanParser(new SpainIbanValidator());

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        string entidad = normalized[4..8];
        string oficina = normalized[8..12];
        string dc = normalized[12..14];
        string cuenta = normalized[14..24];

        return new SpainIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],
            CheckDigits = normalized[2..4],

            BankCode = entidad,
            BranchCode = oficina,
            NationalCheckKey = dc,
            AccountNumber = cuenta,

            Entidad = entidad,
            Oficina = oficina,
            DC = dc,
            Cuenta = cuenta,

            IsValid = true
        };
    }
}
