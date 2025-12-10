using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Turkey.Models;
using Finova.Countries.Europe.Turkey.Validators;

namespace Finova.Countries.Europe.Turkey.Services;

/// <summary>
/// Parser for Turkey IBANs.
/// </summary>
public class TurkeyIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Turkey.
    /// </summary>
    public string CountryCode => "TR";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="TurkeyIbanParser"/> instance.</returns>
    public static TurkeyIbanParser Create() => new TurkeyIbanParser(new TurkeyIbanValidator());

    /// <summary>
    /// Parses the Turkey IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to parse.</param>
    /// <returns>The parsed IBAN details, or null if invalid.</returns>
    public IbanDetails? ParseIban(string? iban)
    {
        if (iban is null || !_validator.Validate(iban).IsValid)
        {
            return null;
        }

        string validIban = iban!.Replace(" ", "").ToUpperInvariant();

        var bankaKodu = validIban.Substring(4, 5);
        var rezervAlan = validIban.Substring(9, 1);
        var hesapNumarasi = validIban.Substring(10, 16);

        return new TurkeyIbanDetails
        {
            Iban = validIban,
            CountryCode = "TR",
            CheckDigits = validIban.Substring(2, 2),

            BankaKodu = bankaKodu,
            RezervAlan = rezervAlan,
            HesapNumarasi = hesapNumarasi,

            BankCode = bankaKodu,
            AccountNumber = hesapNumarasi,
            IsValid = true
        };
    }
}

