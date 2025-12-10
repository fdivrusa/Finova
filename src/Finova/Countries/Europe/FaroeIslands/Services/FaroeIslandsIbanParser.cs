using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.FaroeIslands.Models;
using Finova.Countries.Europe.FaroeIslands.Validators;

namespace Finova.Countries.Europe.FaroeIslands.Services;

/// <summary>
/// Parser for Faroe Islands IBANs.
/// </summary>
public class FaroeIslandsIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Faroe Islands.
    /// </summary>
    public string CountryCode => "FO";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="FaroeIslandsIbanParser"/> instance.</returns>
    public static FaroeIslandsIbanParser Create() => new FaroeIslandsIbanParser(new FaroeIslandsIbanValidator());

    /// <summary>
    /// Parses the Faroe Islands IBAN.
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

        var skrasetingarNummar = validIban.Substring(4, 4);
        var kontoNummar = validIban.Substring(8, 9);
        var eftirlitsTal = validIban.Substring(17, 1);

        return new FaroeIslandsIbanDetails
        {
            Iban = validIban,
            CountryCode = "FO",
            CheckDigits = validIban.Substring(2, 2),

            SkrasetingarNummar = skrasetingarNummar,
            KontoNummar = kontoNummar,
            EftirlitsTal = eftirlitsTal,

            BankCode = skrasetingarNummar,
            AccountNumber = kontoNummar,
            IsValid = true
        };
    }
}

