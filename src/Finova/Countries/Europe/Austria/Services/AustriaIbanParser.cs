using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Austria.Models;
using Finova.Countries.Europe.Austria.Validators;

namespace Finova.Countries.Europe.Austria.Services;

public class AustriaIbanParser(AustriaIbanValidator validator) : IIbanParser
{
    public static AustriaIbanParser Create() => new(new AustriaIbanValidator());
    private readonly AustriaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.IsValidIban(iban))
        {
            return null;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic for Austria:
        // 0-2: Country (AT)
        // 2-4: Check Digits
        // 4-9: Bankleitzahl (Bank Code - 5 digits)
        // 9-20: Kontonummer (Account Number - 11 digits)

        return new AustriaIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],
            CheckDigits = normalized[2..4],

            // Localized Fields
            Bankleitzahl = normalized[4..9],
            Kontonummer = normalized[9..20],

            // Standard Mapping
            BankCode = normalized[4..9],       // Maps to Bankleitzahl
            AccountNumber = normalized[9..20], // Maps to Kontonummer
            IsValid = true
        };
    }
}
