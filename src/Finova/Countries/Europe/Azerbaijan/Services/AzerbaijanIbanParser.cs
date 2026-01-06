using Finova.Core.Iban;

using Finova.Countries.Europe.Azerbaijan.Models;
using Finova.Countries.Europe.Azerbaijan.Validators;

namespace Finova.Countries.Europe.Azerbaijan.Services;

/// <summary>
/// Parser for Azerbaijan IBANs.
/// </summary>
public class AzerbaijanIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Azerbaijan.
    /// </summary>
    public string CountryCode => "AZ";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="AzerbaijanIbanParser"/> instance.</returns>
    public static AzerbaijanIbanParser Create() => new(new AzerbaijanIbanValidator());

    /// <summary>
    /// Parses the Azerbaijan IBAN.
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

        var bankKodu = validIban.Substring(4, 4);
        var hesabNomresi = validIban.Substring(8, 20);

        return new AzerbaijanIbanDetails
        {
            Iban = validIban,
            CountryCode = "AZ",
            CheckDigits = validIban.Substring(2, 2),

            BankKodu = bankKodu,
            HesabNomresi = hesabNomresi,

            BankCode = bankKodu,
            AccountNumber = hesabNomresi,
            IsValid = true
        };
    }
}

