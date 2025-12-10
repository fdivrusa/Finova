using Finova.Core.Iban;


using Finova.Countries.Europe.Slovakia.Models;
using Finova.Countries.Europe.Slovakia.Validators;

namespace Finova.Countries.Europe.Slovakia.Services;

public class SlovakiaIbanParser(SlovakiaIbanValidator validator) : IIbanParser
{
    public static SlovakiaIbanParser Create() => new(new SlovakiaIbanValidator());
    private readonly SlovakiaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new SlovakiaIbanDetails
        {
            Iban = normalized,
            CountryCode = "SK",
            CheckDigits = normalized[2..4],
            KodBanky = normalized[4..8],
            Predcislie = normalized[8..14],
            CisloUctu = normalized[14..24],

            // Generic
            BankCode = normalized[4..8],
            AccountNumber = normalized[14..24],
            IsValid = true
        };
    }
}
