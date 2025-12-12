using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.Albania.Validators;
using Finova.Countries.Europe.Andorra.Validators;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Azerbaijan.Validators;
using Finova.Countries.Europe.Belarus.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Countries.Europe.Croatia.Validators;
using Finova.Countries.Europe.Cyprus.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Estonia.Validators;
using Finova.Countries.Europe.FaroeIslands.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Georgia.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Latvia.Validators;
using Finova.Countries.Europe.Liechtenstein.Validators;
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Moldova.Validators;
using Finova.Countries.Europe.Monaco.Validators;
using Finova.Countries.Europe.Montenegro.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.NorthMacedonia.Validators;
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.SanMarino.Validators;
using Finova.Countries.Europe.Serbia.Validators;
using Finova.Countries.Europe.Slovakia.Validators;
using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.Turkey.Validators;
using Finova.Countries.Europe.Ukraine.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for European VAT numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
public class EuropeVatValidator : IVatValidator
{
    public string CountryCode => "";

    public ValidationResult Validate(string? vat) => ValidateVat(vat);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult ValidateVat(string? vat, string? countryCode = null)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (vat.Length < 2)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number is too short to extract country code.");
            }
            countryCode = vat[0..2];
        }

        // Special handling for Greece (EL/GR) and Switzerland (CHE/CH) if extracted from VAT
        countryCode = countryCode.ToUpperInvariant();

        return countryCode switch
        {
            "AL" => AlbaniaVatValidator.Validate(vat),
            "AD" => AndorraVatValidator.Validate(vat),
            "AT" => AustriaVatValidator.Validate(vat),
            "AZ" => AzerbaijanVatValidator.Validate(vat),
            "BA" => BosniaAndHerzegovinaVatValidator.Validate(vat),
            "BE" => BelgiumVatValidator.Validate(vat),
            "BG" => BulgariaVatValidator.Validate(vat),
            "BY" => BelarusVatValidator.Validate(vat),
            "CHE" or "CH" => SwitzerlandVatValidator.Validate(vat),
            "CY" => CyprusVatValidator.Validate(vat),
            "CZ" => CzechRepublicVatValidator.Validate(vat),
            "DE" => GermanyVatValidator.Validate(vat),
            "DK" => DenmarkVatValidator.Validate(vat),
            "EE" => EstoniaVatValidator.Validate(vat),
            "EL" or "GR" => GreeceVatValidator.Validate(vat),
            "ES" => SpainVatValidator.Validate(vat),
            "FI" => FinlandVatValidator.Validate(vat),
            "FO" => FaroeIslandsVatValidator.Validate(vat),
            "FR" => FranceVatValidator.Validate(vat),
            "GB" => UnitedKingdomVatValidator.Validate(vat),
            "GE" => GeorgiaVatValidator.Validate(vat),
            "HR" => CroatiaVatValidator.Validate(vat),
            "HU" => HungaryVatValidator.Validate(vat),
            "IE" => IrelandVatValidator.Validate(vat),
            "IS" => IcelandVatValidator.Validate(vat),
            "IT" => ItalyVatValidator.Validate(vat),
            "LI" => LiechtensteinVatValidator.Validate(vat),
            "LT" => LithuaniaVatValidator.Validate(vat),
            "LU" => LuxembourgVatValidator.Validate(vat),
            "LV" => LatviaVatValidator.Validate(vat),
            "MC" => MonacoVatValidator.Validate(vat),
            "MD" => MoldovaVatValidator.Validate(vat),
            "ME" => MontenegroVatValidator.Validate(vat),
            "MK" => NorthMacedoniaVatValidator.Validate(vat),
            "MT" => MaltaVatValidator.Validate(vat),
            "NL" => NetherlandsVatValidator.Validate(vat),
            "NO" => NorwayVatValidator.Validate(vat),
            "PL" => PolandVatValidator.Validate(vat),
            "PT" => PortugalVatValidator.Validate(vat),
            "RO" => RomaniaVatValidator.Validate(vat),
            "RS" => SerbiaVatValidator.Validate(vat),
            "SE" => SwedenVatValidator.Validate(vat),
            "SI" => SloveniaVatValidator.Validate(vat),
            "SK" => SlovakiaVatValidator.Validate(vat),
            "SM" => SanMarinoVatValidator.Validate(vat),
            "TR" => TurkeyVatValidator.Validate(vat),
            "UA" => UkraineVatValidator.Validate(vat),

            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Country code {countryCode} is not supported.")
        };
    }

    public static VatDetails? GetVatDetails(string? vat, string? countryCode = null)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (vat.Length < 2)
            {
                return null;
            }
            countryCode = vat[0..2];
        }

        countryCode = countryCode.ToUpperInvariant();

        return countryCode switch
        {
            "AL" => AlbaniaVatValidator.GetVatDetails(vat),
            "AD" => AndorraVatValidator.GetVatDetails(vat),
            "AT" => AustriaVatValidator.GetVatDetails(vat),
            "AZ" => AzerbaijanVatValidator.GetVatDetails(vat),
            "BA" => BosniaAndHerzegovinaVatValidator.GetVatDetails(vat),
            "BE" => BelgiumVatValidator.GetVatDetails(vat),
            "BG" => BulgariaVatValidator.GetVatDetails(vat),
            "BY" => BelarusVatValidator.GetVatDetails(vat),
            "CHE" or "CH" => SwitzerlandVatValidator.GetVatDetails(vat),
            "CY" => CyprusVatValidator.GetVatDetails(vat),
            "CZ" => CzechRepublicVatValidator.GetVatDetails(vat),
            "DE" => GermanyVatValidator.GetVatDetails(vat),
            "DK" => DenmarkVatValidator.GetVatDetails(vat),
            "EE" => EstoniaVatValidator.GetVatDetails(vat),
            "EL" or "GR" => GreeceVatValidator.GetVatDetails(vat),
            "ES" => SpainVatValidator.GetVatDetails(vat),
            "FI" => FinlandVatValidator.GetVatDetails(vat),
            "FO" => FaroeIslandsVatValidator.GetVatDetails(vat),
            "FR" => FranceVatValidator.GetVatDetails(vat),
            "GB" => UnitedKingdomVatValidator.GetVatDetails(vat),
            "GE" => GeorgiaVatValidator.GetVatDetails(vat),
            "HR" => CroatiaVatValidator.GetVatDetails(vat),
            "HU" => HungaryVatValidator.GetVatDetails(vat),
            "IE" => IrelandVatValidator.GetVatDetails(vat),
            "IS" => IcelandVatValidator.GetVatDetails(vat),
            "IT" => ItalyVatValidator.GetVatDetails(vat),
            "LI" => LiechtensteinVatValidator.GetVatDetails(vat),
            "LT" => LithuaniaVatValidator.GetVatDetails(vat),
            "LU" => LuxembourgVatValidator.GetVatDetails(vat),
            "LV" => LatviaVatValidator.GetVatDetails(vat),
            "MC" => MonacoVatValidator.GetVatDetails(vat),
            "MD" => MoldovaVatValidator.GetVatDetails(vat),
            "ME" => MontenegroVatValidator.GetVatDetails(vat),
            "MK" => NorthMacedoniaVatValidator.GetVatDetails(vat),
            "MT" => MaltaVatValidator.GetVatDetails(vat),
            "NL" => NetherlandsVatValidator.GetVatDetails(vat),
            "NO" => NorwayVatValidator.GetVatDetails(vat),
            "PL" => PolandVatValidator.GetVatDetails(vat),
            "PT" => PortugalVatValidator.GetVatDetails(vat),
            "RO" => RomaniaVatValidator.GetVatDetails(vat),
            "RS" => SerbiaVatValidator.GetVatDetails(vat),
            "SE" => SwedenVatValidator.GetVatDetails(vat),
            "SI" => SloveniaVatValidator.GetVatDetails(vat),
            "SK" => SlovakiaVatValidator.GetVatDetails(vat),
            "SM" => SanMarinoVatValidator.GetVatDetails(vat),
            "TR" => TurkeyVatValidator.GetVatDetails(vat),
            "UA" => UkraineVatValidator.GetVatDetails(vat),

            _ => null
        };
    }
}