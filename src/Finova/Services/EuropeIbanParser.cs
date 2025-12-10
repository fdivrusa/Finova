using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Countries.Europe.Andorra.Services;
using Finova.Countries.Europe.Austria.Services;
using Finova.Countries.Europe.Belgium.Services;
using Finova.Countries.Europe.Bulgaria.Services;
using Finova.Countries.Europe.Croatia.Services;
using Finova.Countries.Europe.Cyprus.Services;
using Finova.Countries.Europe.CzechRepublic.Services;
using Finova.Countries.Europe.Denmark.Services;
using Finova.Countries.Europe.Estonia.Services;
using Finova.Countries.Europe.Finland.Services;
using Finova.Countries.Europe.France.Services;
using Finova.Countries.Europe.Germany.Services;
using Finova.Countries.Europe.Gibraltar.Services;
using Finova.Countries.Europe.Greece.Services;
using Finova.Countries.Europe.Hungary.Services;
using Finova.Countries.Europe.Iceland.Services;
using Finova.Countries.Europe.Ireland.Services;
using Finova.Countries.Europe.Italy.Services;
using Finova.Countries.Europe.Latvia.Services;
using Finova.Countries.Europe.Lithuania.Services;
using Finova.Countries.Europe.Luxembourg.Services;
using Finova.Countries.Europe.Malta.Services;
using Finova.Countries.Europe.Monaco.Services;
using Finova.Countries.Europe.Netherlands.Services;
using Finova.Countries.Europe.Norway.Services;
using Finova.Countries.Europe.Poland.Services;
using Finova.Countries.Europe.Portugal.Services;
using Finova.Countries.Europe.Romania.Services;
using Finova.Countries.Europe.SanMarino.Services;
using Finova.Countries.Europe.Slovakia.Services;
using Finova.Countries.Europe.Slovenia.Services;
using Finova.Countries.Europe.Spain.Services;
using Finova.Countries.Europe.Sweden.Services;
using Finova.Countries.Europe.Switzerland.Services;
using Finova.Countries.Europe.UnitedKingdom.Services;
using Finova.Countries.Europe.Vatican.Services;
using Finova.Countries.Europe.Albania.Services;
using Finova.Countries.Europe.Moldova.Services;
using Finova.Countries.Europe.Montenegro.Services;
using Finova.Countries.Europe.NorthMacedonia.Services;
using Finova.Countries.Europe.Liechtenstein.Services;
using Finova.Countries.Europe.Serbia.Services;

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
        if (!EuropeIbanValidator.ValidateIban(normalized).IsValid)
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
            "IE" => IrelandIbanParser.Create().ParseIban(normalized),
            "AT" => AustriaIbanParser.Create().ParseIban(normalized),
            "GR" => GreeceIbanParser.Create().ParseIban(normalized),
            "PT" => PortugalIbanParser.Create().ParseIban(normalized),
            "FI" => FinlandIbanParser.Create().ParseIban(normalized),
            "SE" => SwedenIbanParser.Create().ParseIban(normalized),
            "DK" => DenmarkIbanParser.Create().ParseIban(normalized),
            "NO" => NorwayIbanParser.Create().ParseIban(normalized),
            "PL" => PolandIbanParser.Create().ParseIban(normalized),
            "CZ" => CzechRepublicIbanParser.Create().ParseIban(normalized),
            "HU" => HungaryIbanParser.Create().ParseIban(normalized),
            "RO" => RomaniaIbanParser.Create().ParseIban(normalized),
            "BG" => BulgariaIbanParser.Create().ParseIban(normalized),
            "HR" => CroatiaIbanParser.Create().ParseIban(normalized),
            "SI" => SloveniaIbanParser.Create().ParseIban(normalized),
            "SK" => SlovakiaIbanParser.Create().ParseIban(normalized),
            "EE" => EstoniaIbanParser.Create().ParseIban(normalized),
            "LV" => LatviaIbanParser.Create().ParseIban(normalized),
            "LT" => LithuaniaIbanParser.Create().ParseIban(normalized),
            "CH" => SwitzerlandIbanParser.Create().ParseIban(normalized),
            "CY" => CyprusIbanParser.Create().ParseIban(normalized),
            "MT" => MaltaIbanParser.Create().ParseIban(normalized),
            "MC" => MonacoIbanParser.Create().ParseIban(normalized),
            "AD" => AndorraIbanParser.Create().ParseIban(normalized),
            "VA" => VaticanIbanParser.Create().ParseIban(normalized),
            "SM" => SanMarinoIbanParser.Create().ParseIban(normalized),
            "GI" => GibraltarIbanParser.Create().ParseIban(normalized),
            "IS" => IcelandIbanParser.Create().ParseIban(normalized),
            "AL" => AlbaniaIbanParser.Create().ParseIban(normalized),
            "MD" => MoldovaIbanParser.Create().ParseIban(normalized),
            "ME" => MontenegroIbanParser.Create().ParseIban(normalized),
            "MK" => NorthMacedoniaIbanParser.Create().ParseIban(normalized),
            "LI" => LiechtensteinIbanParser.Create().ParseIban(normalized),
            "RS" => SerbiaIbanParser.Create().ParseIban(normalized),
            _ => new IbanDetails
            {
                Iban = normalized,
                CountryCode = normalized[0..2],
                CheckDigits = normalized[2..4],
                IsValid = true
            }
        };
    }

    public IbanDetails? ParseIban(string? iban) => Parse(iban);
}
