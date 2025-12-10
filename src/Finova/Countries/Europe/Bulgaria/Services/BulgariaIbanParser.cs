using Finova.Core.Iban;


using Finova.Countries.Europe.Bulgaria.Models;
using Finova.Countries.Europe.Bulgaria.Validators;

namespace Finova.Countries.Europe.Bulgaria.Services;

public class BulgariaIbanParser(BulgariaIbanValidator validator) : IIbanParser
{
    public static BulgariaIbanParser Create() => new(new BulgariaIbanValidator());
    private readonly BulgariaIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (!_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // BG IBAN structure:
        // 0-2: BG
        // 2-4: Check Digits
        // 4-8: Bank Code (BIC)
        // 8-12: Branch Code
        // 12-14: Account Type
        // 14-22: Account Number
        return new BulgariaIbanDetails
        {
            Iban = normalized,
            CountryCode = "BG",
            CheckDigits = normalized[2..4],
            BankovKod = normalized[4..8],
            Klon = normalized[8..12],
            VidSmetka = normalized[12..14],
            NomerSmetka = normalized[14..22],

            // Generic Mapping
            BankCode = normalized[4..8],
            BranchCode = normalized[8..12],
            AccountNumber = normalized[12..22],
            IsValid = true
        };
    }
}
