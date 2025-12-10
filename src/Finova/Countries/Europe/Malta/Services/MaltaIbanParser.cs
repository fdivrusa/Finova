using Finova.Core.Iban;


using Finova.Countries.Europe.Malta.Models;
using Finova.Countries.Europe.Malta.Validators;

namespace Finova.Countries.Europe.Malta.Services;

public class MaltaIbanParser(MaltaIbanValidator validator) : IIbanParser
{
    public static MaltaIbanParser Create() => new(new MaltaIbanValidator());
    private readonly MaltaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new MaltaIbanDetails
        {
            Iban = normalized,
            CountryCode = "MT",
            CheckDigits = normalized[2..4],
            BankBic = normalized[4..8],
            SortCode = normalized[8..13],
            AccountNumberMt = normalized[13..31],

            // Generic Mapping
            BankCode = normalized[4..8],
            BranchCode = normalized[8..13],
            AccountNumber = normalized[13..31],
            IsValid = true
        };
    }
}
