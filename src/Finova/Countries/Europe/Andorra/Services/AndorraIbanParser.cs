using Finova.Core.Iban;


using Finova.Countries.Europe.Andorra.Models;
using Finova.Countries.Europe.Andorra.Validators;

namespace Finova.Countries.Europe.Andorra.Services;

public class AndorraIbanParser(AndorraIbanValidator validator) : IIbanParser
{
    public static AndorraIbanParser Create() => new(new AndorraIbanValidator());
    private readonly AndorraIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new AndorraIbanDetails
        {
            Iban = normalized,
            CountryCode = "AD",
            CheckDigits = normalized[2..4],
            CodiEntitat = normalized[4..8],
            CodiOficina = normalized[8..12],
            NumeroCompte = normalized[12..24],

            // Generic Mapping
            BankCode = normalized[4..8],
            BranchCode = normalized[8..12],
            AccountNumber = normalized[12..24],
            IsValid = true
        };
    }
}
