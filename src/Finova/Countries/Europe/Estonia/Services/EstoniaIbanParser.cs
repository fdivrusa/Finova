using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Estonia.Models;
using Finova.Countries.Europe.Estonia.Validators;

namespace Finova.Countries.Europe.Estonia.Services;

public class EstoniaIbanParser(EstoniaIbanValidator validator) : IIbanParser
{
    public static EstoniaIbanParser Create() => new(new EstoniaIbanValidator());
    private readonly EstoniaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new EstoniaIbanDetails
        {
            Iban = normalized,
            CountryCode = "EE",
            CheckDigits = normalized[2..4],
            Pangakood = normalized[4..6],
            Kontonumber = normalized[6..20],

            // Generic Mapping
            BankCode = normalized[4..6],
            AccountNumber = normalized[6..20],
            IsValid = true
        };
    }
}
