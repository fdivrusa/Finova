using Finova.Core.Iban;


using Finova.Countries.Europe.Serbia.Models;
using Finova.Countries.Europe.Serbia.Validators;

namespace Finova.Countries.Europe.Serbia.Services;

public class SerbiaIbanParser(SerbiaIbanValidator validator) : IIbanParser
{
    public static SerbiaIbanParser Create() => new(new SerbiaIbanValidator());
    private readonly SerbiaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new SerbiaIbanDetails
        {
            Iban = normalized,
            CountryCode = "RS",
            CheckDigits = normalized[2..4],
            BrojBanke = normalized[4..7],
            BrojRacuna = normalized[7..20],
            KontrolniBroj = normalized[20..22],

            // Generic Mapping
            BankCode = normalized[4..7],
            AccountNumber = normalized[7..20],
            NationalCheckKey = normalized[20..22],
            IsValid = true
        };
    }
}
