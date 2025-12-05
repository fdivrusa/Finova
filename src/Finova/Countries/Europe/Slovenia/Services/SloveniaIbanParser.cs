using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Slovenia.Models;
using Finova.Countries.Europe.Slovenia.Validators;

namespace Finova.Countries.Europe.Slovenia.Services;

public class SloveniaIbanParser(SloveniaIbanValidator validator) : IIbanParser
{
    public static SloveniaIbanParser Create() => new(new SloveniaIbanValidator());
    private readonly SloveniaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new SloveniaIbanDetails
        {
            Iban = normalized,
            CountryCode = "SI",
            CheckDigits = normalized[2..4],
            StevilkaBanke = normalized[4..9],
            StevilkaRacuna = normalized[9..17],
            KontrolnaStevilka = normalized[17..19],

            // Generic Mapping
            BankCode = normalized[4..9],
            AccountNumber = normalized[9..17],
            NationalCheckKey = normalized[17..19],
            IsValid = true
        };
    }
}
