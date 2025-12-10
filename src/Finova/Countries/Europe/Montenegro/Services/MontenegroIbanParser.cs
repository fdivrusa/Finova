

using Finova.Core.Iban;
using Finova.Countries.Europe.Montenegro.Models;
using Finova.Countries.Europe.Montenegro.Validators;

namespace Finova.Countries.Europe.Montenegro.Services;

/// <summary>
/// Parser for Montenegro IBANs.
/// </summary>
public class MontenegroIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Montenegro.
    /// </summary>
    public string CountryCode => "ME";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="MontenegroIbanParser"/> instance.</returns>
    public static MontenegroIbanParser Create() => new MontenegroIbanParser(new MontenegroIbanValidator());

    /// <summary>
    /// Parses the Montenegro IBAN.
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

        var sifraBanke = validIban.Substring(4, 3);
        var brojRacuna = validIban.Substring(7, 13);
        var kontrolniBroj = validIban.Substring(20, 2);

        return new MontenegroIbanDetails
        {
            Iban = validIban,
            CountryCode = "ME",
            CheckDigits = validIban.Substring(2, 2),

            SifraBanke = sifraBanke,
            BrojRacuna = brojRacuna,
            KontrolniBroj = kontrolniBroj,

            BankCode = sifraBanke,
            AccountNumber = brojRacuna,
            IsValid = true
        };
    }
}
