using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Belarus.Models;
using Finova.Countries.Europe.Belarus.Validators;

namespace Finova.Countries.Europe.Belarus.Services;

/// <summary>
/// Parser for Belarus IBANs.
/// </summary>
public class BelarusIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Belarus.
    /// </summary>
    public string CountryCode => "BY";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="BelarusIbanParser"/> instance.</returns>
    public static BelarusIbanParser Create() => new BelarusIbanParser(new BelarusIbanValidator());

    /// <summary>
    /// Parses the Belarus IBAN.
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

        var kodBanku = validIban.Substring(4, 4);
        var balansovyRahunak = validIban.Substring(8, 4);
        var numarRahunku = validIban.Substring(12, 16);

        return new BelarusIbanDetails
        {
            Iban = validIban,
            CountryCode = "BY",
            CheckDigits = validIban.Substring(2, 2),

            KodBanku = kodBanku,
            BalansovyRahunak = balansovyRahunak,
            NumarRahunku = numarRahunku,

            BankCode = kodBanku,
            AccountNumber = numarRahunku,
            IsValid = true
        };
    }
}

