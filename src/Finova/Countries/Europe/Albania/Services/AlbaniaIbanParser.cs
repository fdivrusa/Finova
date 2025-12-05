using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Albania.Models;
using Finova.Countries.Europe.Albania.Validators;

namespace Finova.Countries.Europe.Albania.Services;

/// <summary>
/// Parser for Albanian IBANs.
/// </summary>
public class AlbaniaIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Albania.
    /// </summary>
    public string CountryCode => "AL";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="AlbaniaIbanParser"/> instance.</returns>
    public static AlbaniaIbanParser Create()
    {
        return new AlbaniaIbanParser(new AlbaniaIbanValidator());
    }

    /// <summary>
    /// Parses the Albanian IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to parse.</param>
    /// <returns>The parsed IBAN details, or null if invalid.</returns>
    public IbanDetails? ParseIban(string? iban)
    {
        if (iban is null || !_validator.IsValidIban(iban))
        {
            return null;
        }

        // We know it's valid and not null here
        string validIban = iban!.Replace(" ", "").ToUpperInvariant();

        var kodiBankes = validIban.Substring(4, 3);
        var kodiDeges = validIban.Substring(7, 4);
        var shifraKontrollit = validIban.Substring(11, 1);
        var numriLlogarise = validIban.Substring(12, 16);

        return new AlbaniaIbanDetails
        {
            Iban = validIban,
            CountryCode = "AL",
            CheckDigits = validIban.Substring(2, 2),

            KodiBankes = kodiBankes,
            KodiDeges = kodiDeges,
            ShifraKontrollit = shifraKontrollit,
            NumriLlogarise = numriLlogarise,

            BankCode = kodiBankes,
            BranchCode = kodiDeges,
            AccountNumber = numriLlogarise,
            IsValid = true
        };
    }
}
