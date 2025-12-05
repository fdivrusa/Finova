using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Hungary.Models;
using Finova.Countries.Europe.Hungary.Validators;

namespace Finova.Countries.Europe.Hungary.Services;

public class HungaryIbanParser(HungaryIbanValidator validator) : IIbanParser
{
    public static HungaryIbanParser Create() => new(new HungaryIbanValidator());
    private readonly HungaryIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new HungaryIbanDetails
        {
            Iban = normalized,
            CountryCode = "HU",
            CheckDigits = normalized[2..4],
            Bankazonosito = normalized[4..7],
            Fiokazonosito = normalized[7..11],
            Szamlaszam = normalized[11..28],

            // Generic Mapping
            BankCode = normalized[4..7],
            BranchCode = normalized[7..11],
            AccountNumber = normalized[11..28],
            IsValid = true
        };
    }
}
