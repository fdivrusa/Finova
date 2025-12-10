using Finova.Core.Iban;


using Finova.Countries.Europe.Lithuania.Models;
using Finova.Countries.Europe.Lithuania.Validators;

namespace Finova.Countries.Europe.Lithuania.Services;

public class LithuaniaIbanParser(LithuaniaIbanValidator validator) : IIbanParser
{
    public static LithuaniaIbanParser Create() => new(new LithuaniaIbanValidator());
    private readonly LithuaniaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new LithuaniaIbanDetails
        {
            Iban = normalized,
            CountryCode = "LT",
            CheckDigits = normalized[2..4],
            BankoKodas = normalized[4..9],
            SaskaitosNumeris = normalized[9..20],

            // Generic Mapping
            BankCode = normalized[4..9],
            AccountNumber = normalized[9..20],
            IsValid = true
        };
    }
}
