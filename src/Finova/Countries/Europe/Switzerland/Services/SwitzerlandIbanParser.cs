using Finova.Core.Iban;


using Finova.Countries.Europe.Switzerland.Models;
using Finova.Countries.Europe.Switzerland.Validators;

namespace Finova.Countries.Europe.Switzerland.Services;

public class SwitzerlandIbanParser(SwitzerlandIbanValidator validator) : IIbanParser
{
    public static SwitzerlandIbanParser Create() => new(new SwitzerlandIbanValidator());
    private readonly SwitzerlandIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new SwitzerlandIbanDetails
        {
            Iban = normalized,
            CountryCode = "CH",
            CheckDigits = normalized[2..4],
            ClearingNummer = normalized[4..9],
            KontoNummer = normalized[9..21],

            // Generic Mapping
            BankCode = normalized[4..9],
            AccountNumber = normalized[9..21],
            IsValid = true
        };
    }
}
