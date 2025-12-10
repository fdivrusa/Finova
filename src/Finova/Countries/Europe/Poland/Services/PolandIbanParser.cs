using Finova.Core.Iban;


using Finova.Countries.Europe.Poland.Models;
using Finova.Countries.Europe.Poland.Validators;

namespace Finova.Countries.Europe.Poland.Services;

public class PolandIbanParser(PolandIbanValidator validator) : IIbanParser
{
    public static PolandIbanParser Create() => new(new PolandIbanValidator());
    private readonly PolandIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new PolandIbanDetails
        {
            Iban = normalized,
            CountryCode = "PL",
            CheckDigits = normalized[2..4],
            NumerRozliczeniowyBanku = normalized[4..12],
            NumerRachunku = normalized[12..28],

            // Generic Mapping
            BankCode = normalized[4..12],
            AccountNumber = normalized[12..28],
            IsValid = true
        };
    }
}
