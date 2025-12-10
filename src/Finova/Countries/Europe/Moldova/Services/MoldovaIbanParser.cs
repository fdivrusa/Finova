

using Finova.Core.Iban;
using Finova.Countries.Europe.Moldova.Models;
using Finova.Countries.Europe.Moldova.Validators;

namespace Finova.Countries.Europe.Moldova.Services;

/// <summary>
/// Parser for Moldovan IBANs.
/// </summary>
public class MoldovaIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Moldova.
    /// </summary>
    public string CountryCode => "MD";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="MoldovaIbanParser"/> instance.</returns>
    public static MoldovaIbanParser Create() => new MoldovaIbanParser(new MoldovaIbanValidator());

    /// <summary>
    /// Parses the Moldovan IBAN.
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

        var codBanca = validIban.Substring(4, 2);
        var numarCont = validIban.Substring(6, 18);

        return new MoldovaIbanDetails
        {
            Iban = validIban,
            CountryCode = "MD",
            CheckDigits = validIban.Substring(2, 2),

            CodBanca = codBanca,
            NumarCont = numarCont,

            BankCode = codBanca,
            AccountNumber = numarCont,
            IsValid = true
        };
    }
}
