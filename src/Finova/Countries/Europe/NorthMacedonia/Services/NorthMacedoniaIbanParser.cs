

using Finova.Core.Iban;
using Finova.Countries.Europe.NorthMacedonia.Models;
using Finova.Countries.Europe.NorthMacedonia.Validators;

namespace Finova.Countries.Europe.NorthMacedonia.Services;

/// <summary>
/// Parser for North Macedonia IBANs.
/// </summary>
public class NorthMacedoniaIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for North Macedonia.
    /// </summary>
    public string CountryCode => "MK";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="NorthMacedoniaIbanParser"/> instance.</returns>
    public static NorthMacedoniaIbanParser Create() => new NorthMacedoniaIbanParser(new NorthMacedoniaIbanValidator());

    /// <summary>
    /// Parses the North Macedonia IBAN.
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

        var sifraBanka = validIban.Substring(4, 3);
        var brojSmetka = validIban.Substring(7, 10);
        var kontrolenBroj = validIban.Substring(17, 2);

        return new NorthMacedoniaIbanDetails
        {
            Iban = validIban,
            CountryCode = "MK",
            CheckDigits = validIban.Substring(2, 2),

            SifraBanka = sifraBanka,
            BrojSmetka = brojSmetka,
            KontrolenBroj = kontrolenBroj,

            BankCode = sifraBanka,
            AccountNumber = brojSmetka,
            IsValid = true
        };
    }
}
