using Finova.Core.Iban;


using Finova.Countries.Europe.Croatia.Models;
using Finova.Countries.Europe.Croatia.Validators;

namespace Finova.Countries.Europe.Croatia.Services;

public class CroatiaIbanParser(CroatiaIbanValidator validator) : IIbanParser
{
    public static CroatiaIbanParser Create() => new(new CroatiaIbanValidator());
    private readonly CroatiaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new CroatiaIbanDetails
        {
            Iban = normalized,
            CountryCode = "HR",
            CheckDigits = normalized[2..4],
            VodeciBrojBanke = normalized[4..11],
            BrojRacuna = normalized[11..21],

            // Generic Mapping
            BankCode = normalized[4..11],
            AccountNumber = normalized[11..21],
            IsValid = true
        };
    }
}
