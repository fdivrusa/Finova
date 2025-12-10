using Finova.Core.Iban;


using Finova.Countries.Europe.Gibraltar.Models;
using Finova.Countries.Europe.Gibraltar.Validators;

namespace Finova.Countries.Europe.Gibraltar.Services;

public class GibraltarIbanParser(GibraltarIbanValidator validator) : IIbanParser
{
    public static GibraltarIbanParser Create() => new(new GibraltarIbanValidator());
    private readonly GibraltarIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new GibraltarIbanDetails
        {
            Iban = normalized,
            CountryCode = "GI",
            CheckDigits = normalized[2..4],
            BankCodeGi = normalized[4..8],
            AccountNumberGi = normalized[8..23],

            // Generic Mapping
            BankCode = normalized[4..8],
            AccountNumber = normalized[8..23],
            IsValid = true
        };
    }
}
