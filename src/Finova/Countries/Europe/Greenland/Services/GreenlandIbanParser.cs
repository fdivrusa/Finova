using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Greenland.Models;
using Finova.Countries.Europe.Greenland.Validators;

namespace Finova.Countries.Europe.Greenland.Services;

/// <summary>
/// Parser for Greenland IBANs.
/// </summary>
public class GreenlandIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Greenland.
    /// </summary>
    public string CountryCode => "GL";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="GreenlandIbanParser"/> instance.</returns>
    public static GreenlandIbanParser Create() => new GreenlandIbanParser(new GreenlandIbanValidator());

    /// <summary>
    /// Parses the Greenland IBAN.
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

        var bankKode = validIban.Substring(4, 4);
        var kontoNummer = validIban.Substring(8, 10);

        return new GreenlandIbanDetails
        {
            Iban = validIban,
            CountryCode = "GL",
            CheckDigits = validIban.Substring(2, 2),

            BankKode = bankKode,
            KontoNummer = kontoNummer,

            BankCode = bankKode,
            AccountNumber = kontoNummer,
            IsValid = true
        };
    }
}

