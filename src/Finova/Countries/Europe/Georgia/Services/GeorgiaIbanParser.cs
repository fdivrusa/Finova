using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Georgia.Models;
using Finova.Countries.Europe.Georgia.Validators;

namespace Finova.Countries.Europe.Georgia.Services;

/// <summary>
/// Parser for Georgia IBANs.
/// </summary>
public class GeorgiaIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Georgia.
    /// </summary>
    public string CountryCode => "GE";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="GeorgiaIbanParser"/> instance.</returns>
    public static GeorgiaIbanParser Create() => new GeorgiaIbanParser(new GeorgiaIbanValidator());

    /// <summary>
    /// Parses the Georgia IBAN.
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

        var bankisKodi = validIban.Substring(4, 2);
        var angarishisNomeri = validIban.Substring(6, 16);

        return new GeorgiaIbanDetails
        {
            Iban = validIban,
            CountryCode = "GE",
            CheckDigits = validIban.Substring(2, 2),

            BankisKodi = bankisKodi,
            AngarishisNomeri = angarishisNomeri,

            BankCode = bankisKodi,
            AccountNumber = angarishisNomeri,
            IsValid = true
        };
    }
}

