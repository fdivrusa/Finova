using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Kosovo.Models;
using Finova.Countries.Europe.Kosovo.Validators;

namespace Finova.Countries.Europe.Kosovo.Services;

/// <summary>
/// Parser for Kosovo IBANs.
/// </summary>
public class KosovoIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Kosovo.
    /// </summary>
    public string CountryCode => "XK";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="KosovoIbanParser"/> instance.</returns>
    public static KosovoIbanParser Create() => new KosovoIbanParser(new KosovoIbanValidator());

    /// <summary>
    /// Parses the Kosovo IBAN.
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

        var kodiBankes = validIban.Substring(4, 2);
        var kodiDeges = validIban.Substring(6, 2);
        var numriLlogarise = validIban.Substring(8, 10);
        var shifraKontrollit = validIban.Substring(18, 2);

        return new KosovoIbanDetails
        {
            Iban = validIban,
            CountryCode = "XK",
            CheckDigits = validIban.Substring(2, 2),

            KodiBankes = kodiBankes,
            KodiDeges = kodiDeges,
            NumriLlogarise = numriLlogarise,
            ShifraKontrollit = shifraKontrollit,

            BankCode = kodiBankes,
            BranchCode = kodiDeges,
            AccountNumber = numriLlogarise,
            IsValid = true
        };
    }
}

