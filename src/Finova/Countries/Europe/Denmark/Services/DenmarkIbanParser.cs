using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Denmark.Models;
using Finova.Countries.Europe.Denmark.Validators;

namespace Finova.Countries.Europe.Denmark.Services;

public class DenmarkIbanParser(DenmarkIbanValidator validator) : IIbanParser
{
    public static DenmarkIbanParser Create() => new(new DenmarkIbanValidator());
    private readonly DenmarkIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new DenmarkIbanDetails
        {
            Iban = normalized,
            CountryCode = "DK",
            CheckDigits = normalized[2..4],
            Registreringsnummer = normalized[4..8],
            Kontonummer = normalized[8..18],

            // Generic Mapping
            BankCode = normalized[4..8],
            AccountNumber = normalized[8..18],
            IsValid = true
        };
    }
}
