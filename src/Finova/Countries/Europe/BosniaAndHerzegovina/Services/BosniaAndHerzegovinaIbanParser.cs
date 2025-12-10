using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.BosniaAndHerzegovina.Models;
using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Services;

/// <summary>
/// Parser for Bosnia and Herzegovina IBANs.
/// </summary>
public class BosniaAndHerzegovinaIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Bosnia and Herzegovina.
    /// </summary>
    public string CountryCode => "BA";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="BosniaAndHerzegovinaIbanParser"/> instance.</returns>
    public static BosniaAndHerzegovinaIbanParser Create() => new BosniaAndHerzegovinaIbanParser(new BosniaAndHerzegovinaIbanValidator());

    /// <summary>
    /// Parses the Bosnia and Herzegovina IBAN.
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

        var brojBanke = validIban.Substring(4, 3);
        var brojFilijale = validIban.Substring(7, 3);
        var brojRacuna = validIban.Substring(10, 8);
        var kontrolniBroj = validIban.Substring(18, 2);

        return new BosniaAndHerzegovinaIbanDetails
        {
            Iban = validIban,
            CountryCode = "BA",
            CheckDigits = validIban.Substring(2, 2),

            BrojBanke = brojBanke,
            BrojFilijale = brojFilijale,
            BrojRacuna = brojRacuna,
            KontrolniBroj = kontrolniBroj,

            BankCode = brojBanke,
            BranchCode = brojFilijale,
            AccountNumber = brojRacuna,
            IsValid = true
        };
    }
}

