using Finova.Core.Iban;


using Finova.Countries.Europe.Luxembourg.Models;
using Finova.Countries.Europe.Luxembourg.Validators;

namespace Finova.Countries.Europe.Luxembourg.Services;

public class LuxembourgIbanParser(LuxembourgIbanValidator validator) : IIbanParser
{
    private readonly LuxembourgIbanValidator _validator = validator;

    public string? CountryCode => _validator.CountryCode;

    //static method
    public static LuxembourgIbanParser Create() => new LuxembourgIbanParser(new LuxembourgIbanValidator());

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        return new LuxembourgIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],     // "LU"
            CheckDigits = normalized[2..4],     // Check digits

            // Specific properties
            BankCodeLu = normalized[4..7],  // 3 digits
            AccountNumberLu = normalized[7..20], // 13 alphanumeric characters

            // Generic properties (Mapping)
            BankCode = normalized[4..7],
            AccountNumber = normalized[7..20],

            IsValid = true
        };
    }
}
