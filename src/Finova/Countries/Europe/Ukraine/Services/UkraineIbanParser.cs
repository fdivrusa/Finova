using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Ukraine.Models;
using Finova.Countries.Europe.Ukraine.Validators;

namespace Finova.Countries.Europe.Ukraine.Services;

/// <summary>
/// Parser for Ukraine IBANs.
/// </summary>
public class UkraineIbanParser(IIbanValidator validator) : IIbanParser
{
    private readonly IIbanValidator _validator = validator;

    /// <summary>
    /// Gets the country code for Ukraine.
    /// </summary>
    public string CountryCode => "UA";

    /// <summary>
    /// Creates a new instance of the parser with a default validator.
    /// </summary>
    /// <returns>A new <see cref="UkraineIbanParser"/> instance.</returns>
    public static UkraineIbanParser Create() => new UkraineIbanParser(new UkraineIbanValidator());

    /// <summary>
    /// Parses the Ukraine IBAN.
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

        var kodBanku = validIban.Substring(4, 6);
        var nomeraRahunku = validIban.Substring(10, 19);

        return new UkraineIbanDetails
        {
            Iban = validIban,
            CountryCode = "UA",
            CheckDigits = validIban.Substring(2, 2),

            KodBanku = kodBanku,
            NomeraRahunku = nomeraRahunku,

            BankCode = kodBanku,
            AccountNumber = nomeraRahunku,
            IsValid = true
        };
    }
}

