
using Finova.Countries.Europe.Iceland.Models;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;


namespace Finova.Countries.Europe.Iceland.Services;

public class IcelandIbanParser(IcelandIbanValidator validator) : IIbanParser
{
    public static IcelandIbanParser Create() => new(new IcelandIbanValidator());
    private readonly IcelandIbanValidator _validator = validator;
    public string? CountryCode => _validator.CountryCode;

    public IbanDetails? ParseIban(string? iban)
    {
        if (iban is null || !_validator.Validate(iban).IsValid)
        {
            return null;
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        return new IcelandIbanDetails
        {
            Iban = normalized,
            CountryCode = "IS",
            CheckDigits = normalized[2..4],
            BankCode = normalized.Substring(4, 4),
            BranchCode = normalized.Substring(8, 2),
            AccountNumber = normalized.Substring(10, 6),
            Kennitala = normalized.Substring(16, 10),

            // Iceland specific properties
            Banki = normalized.Substring(4, 4),
            Hb = normalized.Substring(8, 2),
            Reikningsnumer = normalized.Substring(10, 6),

            IsValid = true
        };
    }
}

