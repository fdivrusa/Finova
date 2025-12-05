using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Monaco.Models;
using Finova.Countries.Europe.Monaco.Validators;

namespace Finova.Countries.Europe.Monaco.Services;

public class MonacoIbanParser(MonacoIbanValidator validator) : IIbanParser
{
    public static MonacoIbanParser Create() => new(new MonacoIbanValidator());
    private readonly MonacoIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new MonacoIbanDetails
        {
            Iban = normalized,
            CountryCode = "MC",
            CheckDigits = normalized[2..4],
            CodeBanque = normalized[4..9],
            CodeGuichet = normalized[9..14],
            NumeroCompte = normalized[14..25],
            CleRib = normalized[25..27],

            // Generic Mapping
            BankCode = normalized[4..9],
            BranchCode = normalized[9..14],
            AccountNumber = normalized[14..25],
            NationalCheckKey = normalized[25..27],
            IsValid = true
        };
    }
}
