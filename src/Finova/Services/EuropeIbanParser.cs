using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;
using Finova.Countries.Europe.Belgium.Services;
using Finova.Countries.Europe.France.Services;
using Finova.Countries.Europe.Germany.Services;
using Finova.Countries.Europe.Italy.Services;
using Finova.Countries.Europe.Luxembourg.Services;
using Finova.Countries.Europe.Netherlands.Services;
using Finova.Countries.Europe.Spain.Services;
using Finova.Countries.Europe.UnitedKingdom.Services;

namespace Finova.Services;

public class EuropeIbanParser : IIbanParser
{
    public string? CountryCode => null;

    public static IbanDetails? Parse(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban) || iban.Length < 2)
        {
            return null;
        }

        // 1. Normalize & Validate First (Fail fast)
        var normalized = IbanHelper.NormalizeIban(iban);
        if (!EuropeIbanValidator.Validate(normalized))
        {
            return null;
        }

        string country = normalized[0..2];

        return country switch
        {
            "DE" => GermanyIbanParser.Create().ParseIban(normalized),
            "IT" => ItalyIbanParser.Create().ParseIban(normalized),
            "ES" => SpainIbanParser.Create().ParseIban(normalized),
            "FR" => FranceIbanParser.Create().ParseIban(normalized),
            "BE" => BelgiumIbanParser.Create().ParseIban(normalized),
            "LU" => LuxembourgIbanParser.Create().ParseIban(normalized),
            "GB" => UnitedKingdomIbanParser.Create().ParseIban(normalized),
            "NL" => NetherlandsIbanParser.Create().ParseIban(normalized),

            _ => new IbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],
                CheckDigits = normalized[2..4],
                IsValid = true
            }
        };
    }

    public IbanDetails? ParseIban(string? iban)
    {
        return Parse(iban);
    }
}
