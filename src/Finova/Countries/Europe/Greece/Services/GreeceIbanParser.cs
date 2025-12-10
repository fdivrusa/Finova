using Finova.Core.Iban;


using Finova.Countries.Europe.Greece.Models;
using Finova.Countries.Europe.Greece.Validators;

namespace Finova.Countries.Europe.Greece.Services;

public class GreeceIbanParser(GreeceIbanValidator validator) : IIbanParser
{
    public static GreeceIbanParser Create() => new(new GreeceIbanValidator());
    private readonly GreeceIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Parsing logic for Greece:
        // 0-2: Country (GR)
        // 2-4: Check Digits
        // 4-7: Kodikos Trapezas (Bank - 3 digits)
        // 7-11: Kodikos Katastimatos (Branch - 4 digits)
        // 11-27: Arithmos Logariasmou (Account - 16 alphanumeric)

        return new GreeceIbanDetails
        {
            Iban = normalized,
            CountryCode = normalized[0..2],
            CheckDigits = normalized[2..4],

            // Localized Fields
            KodikosTrapezas = normalized[4..7],
            KodikosKatastimatos = normalized[7..11],
            ArithmosLogariasmou = normalized[11..27],

            // Standard Mapping
            BankCode = normalized[4..7],        // Maps to KodikosTrapezas
            BranchCode = normalized[7..11],     // Maps to KodikosKatastimatos
            AccountNumber = normalized[11..27], // Maps to ArithmosLogariasmou
            IsValid = true
        };
    }
}
