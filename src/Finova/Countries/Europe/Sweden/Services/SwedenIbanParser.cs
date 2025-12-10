using Finova.Core.Iban;


using Finova.Countries.Europe.Sweden.Models;
using Finova.Countries.Europe.Sweden.Validators;

namespace Finova.Countries.Europe.Sweden.Services;

public class SwedenIbanParser(SwedenIbanValidator validator) : IIbanParser
{
    public static SwedenIbanParser Create() => new(new SwedenIbanValidator());
    private readonly SwedenIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new SwedenIbanDetails
        {
            Iban = normalized,
            CountryCode = "SE",
            CheckDigits = normalized[2..4],
            Bankkod = normalized[4..7],
            Kontonummer = normalized[7..24],

            // Generic Mapping
            BankCode = normalized[4..7],
            AccountNumber = normalized[7..24],
            IsValid = true
        };
    }
}
