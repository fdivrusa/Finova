using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Liechtenstein.Models;
using Finova.Countries.Europe.Liechtenstein.Validators;

namespace Finova.Countries.Europe.Liechtenstein.Services;

/// <summary>
/// Parser for Liechtenstein IBANs.
/// </summary>
public class LiechtensteinIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Liechtenstein.
    /// </summary>
    public string CountryCode => "LI";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="LiechtensteinIbanParser"/> instance.</returns>
    public static LiechtensteinIbanParser Create()
    {
        return new LiechtensteinIbanParser(new LiechtensteinIbanValidator());
    }

    /// <summary>
    /// Parses the Liechtenstein IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to parse.</param>
    /// <returns>The parsed IBAN details, or null if invalid.</returns>
    public IbanDetails? ParseIban(string? iban)
    {
        if (iban is null || !_validator.IsValidIban(iban))
        {
            return null;
        }

        string validIban = iban!.Replace(" ", "").ToUpperInvariant();

        var bankleitzahl = validIban.Substring(4, 5);
        var kontonummer = validIban.Substring(9, 12);

        return new LiechtensteinIbanDetails
        {
            Iban = validIban,
            CountryCode = "LI",
            CheckDigits = validIban.Substring(2, 2),

            Bankleitzahl = bankleitzahl,
            Kontonummer = kontonummer,

            BankCode = bankleitzahl,
            AccountNumber = kontonummer,
            IsValid = true
        };
    }
}
