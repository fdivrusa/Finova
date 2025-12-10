using Finova.Core.Iban;
using Finova.Countries.Europe.France.Models;
using Finova.Countries.Europe.France.Validators;

namespace Finova.Countries.Europe.France.Services;

public class FranceIbanParser(FranceIbanValidator validator) : IIbanParser
{
    private readonly FranceIbanValidator _validator = validator;

    public string? CountryCode => _validator.CountryCode;

    public static FranceIbanParser Create() => new FranceIbanParser(new FranceIbanValidator());

    public IbanDetails? ParseIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return null;
        }

        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new FranceIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],
            CheckDigits = normalized[2..4],

            BankCodeFr = normalized[4..9],
            BranchCodeFr = normalized[9..14],
            AccountNumberFr = normalized[14..25],
            RibKey = normalized[25..27],

            BankCode = normalized[4..9],
            BranchCode = normalized[9..14],
            AccountNumber = normalized[14..25],
            NationalCheckKey = normalized[25..27],

            IsValid = true
        };
    }
}
