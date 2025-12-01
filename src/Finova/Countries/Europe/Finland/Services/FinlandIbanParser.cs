using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Finland.Models;
using Finova.Countries.Europe.Finland.Validators;

namespace Finova.Countries.Europe.Finland.Services;

public class FinlandIbanParser(FinlandIbanValidator validator) : IIbanParser
{
    public static FinlandIbanParser Create() => new(new FinlandIbanValidator());
    private readonly FinlandIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic for Finland:
        // 0-2: Country (FI)
        // 2-4: Check Digits
        // 4-10: Rahalaitostunnus (Bank - 6 digits)
        // 10-18: Tilinumero (Account - 8 digits)

        return new FinlandIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],
            CheckDigits = normalized[2..4],

            // Localized Fields
            Rahalaitostunnus = normalized[4..10],
            Tilinumero = normalized[10..18],

            // Standard Mapping
            BankCode = normalized[4..10],     // Maps to Rahalaitostunnus
            AccountNumber = normalized[10..18], // Maps to Tilinumero
            IsValid = true
        };
    }
}
