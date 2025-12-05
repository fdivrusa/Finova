using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Cyprus.Models;
using Finova.Countries.Europe.Cyprus.Validators;

namespace Finova.Countries.Europe.Cyprus.Services;

public class CyprusIbanParser(CyprusIbanValidator validator) : IIbanParser
{
    public static CyprusIbanParser Create() => new(new CyprusIbanValidator());
    private readonly CyprusIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new CyprusIbanDetails
        {
            Iban = normalized,
            CountryCode = "CY",
            CheckDigits = normalized[2..4],
            BankCodeCy = normalized[4..7],
            BranchCodeCy = normalized[7..12],
            AccountNumberCy = normalized[12..28],

            // Generic Mapping
            BankCode = normalized[4..7],
            BranchCode = normalized[7..12],
            AccountNumber = normalized[12..28],
            IsValid = true
        };
    }
}
