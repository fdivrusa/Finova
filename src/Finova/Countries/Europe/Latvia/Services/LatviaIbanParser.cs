using Finova.Core.Iban;


using Finova.Countries.Europe.Latvia.Models;
using Finova.Countries.Europe.Latvia.Validators;

namespace Finova.Countries.Europe.Latvia.Services;

public class LatviaIbanParser(LatviaIbanValidator validator) : IIbanParser
{
    public static LatviaIbanParser Create() => new(new LatviaIbanValidator());
    private readonly LatviaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new LatviaIbanDetails
        {
            Iban = normalized,
            CountryCode = "LV",
            CheckDigits = normalized[2..4],
            BankasKods = normalized[4..8],
            KlientaKontaNumurs = normalized[8..21],

            // Generic Mapping
            BankCode = normalized[4..8],
            AccountNumber = normalized[8..21],
            IsValid = true
        };
    }
}
