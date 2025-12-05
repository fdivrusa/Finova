using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Vatican.Models;
using Finova.Countries.Europe.Vatican.Validators;

namespace Finova.Countries.Europe.Vatican.Services;

public class VaticanIbanParser(VaticanIbanValidator validator) : IIbanParser
{
    public static VaticanIbanParser Create() => new(new VaticanIbanValidator());
    private readonly VaticanIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new VaticanIbanDetails
        {
            Iban = normalized,
            CountryCode = "VA",
            CheckDigits = normalized[2..4],
            CodiceBanca = normalized[4..7],
            NumeroConto = normalized[7..22],

            // Generic Mapping
            BankCode = normalized[4..7],
            AccountNumber = normalized[7..22],
            IsValid = true
        };
    }
}
