using Finova.Core.Common;
using Finova.Core.Iban;
// Africa
using Finova.Countries.Africa.Algeria.Validators;
using Finova.Countries.Africa.Angola.Validators;
using Finova.Countries.Africa.Benin.Validators;
using Finova.Countries.Africa.BurkinaFaso.Validators;
using Finova.Countries.Africa.Burundi.Validators;
using Finova.Countries.Africa.Cameroon.Validators;
using Finova.Countries.Africa.CapeVerde.Validators;
using Finova.Countries.Africa.CentralAfricanRepublic.Validators;
using Finova.Countries.Africa.Chad.Validators;
using Finova.Countries.Africa.Comoros.Validators;
using Finova.Countries.Africa.Congo.Validators;
using Finova.Countries.Africa.CoteDIvoire.Validators;
using Finova.Countries.Africa.Djibouti.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.EquatorialGuinea.Validators;
using Finova.Countries.Africa.Gabon.Validators;
using Finova.Countries.Africa.GuineaBissau.Validators;
using Finova.Countries.Africa.Libya.Validators;
using Finova.Countries.Africa.Madagascar.Validators;
using Finova.Countries.Africa.Mali.Validators;
using Finova.Countries.Africa.Mauritania.Validators;
using Finova.Countries.Africa.Morocco.Validators;
using Finova.Countries.Africa.Mozambique.Validators;
using Finova.Countries.Africa.Niger.Validators;
using Finova.Countries.Africa.SaoTomeAndPrincipe.Validators;
using Finova.Countries.Africa.Senegal.Validators;
using Finova.Countries.Africa.Seychelles.Validators;
using Finova.Countries.Africa.Somalia.Validators;
using Finova.Countries.Africa.Sudan.Validators;
using Finova.Countries.Africa.Togo.Validators;
using Finova.Countries.Africa.Tunisia.Validators;
// Asia
using Finova.Countries.Asia.Kazakhstan.Validators;
using Finova.Countries.Asia.Mongolia.Validators;
using Finova.Countries.Asia.Pakistan.Validators;
using Finova.Countries.Asia.TimorLeste.Validators;
// Europe (Non-EU/Other)
using Finova.Countries.Europe.Russia.Validators;
// Middle East
using Finova.Countries.MiddleEast.Bahrain.Validators;
using Finova.Countries.MiddleEast.Iraq.Validators;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.Jordan.Validators;
using Finova.Countries.MiddleEast.Kuwait.Validators;
using Finova.Countries.MiddleEast.Lebanon.Validators;
using Finova.Countries.MiddleEast.Oman.Validators;
using Finova.Countries.MiddleEast.Palestine.Validators;
using Finova.Countries.MiddleEast.Qatar.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Finova.Countries.MiddleEast.Yemen.Validators;
using Finova.Countries.NorthAmerica.Barbados.Validators;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using Finova.Countries.NorthAmerica.Honduras.Validators;
using Finova.Countries.NorthAmerica.Nicaragua.Validators;
using Finova.Countries.NorthAmerica.SaintLucia.Validators;
using Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;
// Americas
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.FalklandIslands.Validators;

namespace Finova.Services;

/// <summary>
/// Global IBAN validator that supports all IBAN-enabled countries worldwide.
/// Routes validation to regional validators (Europe, Middle East, Africa, Americas, Asia).
/// </summary>
/// <example>
/// <code>
/// // Validate any IBAN worldwide
/// var result = GlobalIbanValidator.ValidateIban("BR1800360305000010009795493C1");
///
/// // European IBANs are routed to EuropeIbanValidator
/// var resultEu = GlobalIbanValidator.ValidateIban("DE89370400440532013000");
/// </code>
/// </example>
public static class GlobalIbanValidator
{
    /// <summary>
    /// Validates an IBAN from any supported country.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>A ValidationResult indicating success or failure with detailed error information.</returns>
    public static ValidationResult ValidateIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (iban.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InvalidLength);
        }

        string country = IbanHelper.NormalizeIban(iban)[0..2].ToUpperInvariant();

        return country switch
        {
            // ===== EUROPE (delegated to EuropeIbanValidator) =====
            "AL" or "AD" or "AT" or "AZ" or "BA" or "BY" or "BE" or "BG" or "HR" or "CY" or
            "CZ" or "DK" or "EE" or "FO" or "FI" or "FR" or "GE" or "DE" or "GI" or "GR" or
            "GL" or "HU" or "IS" or "IE" or "IT" or "XK" or "LV" or "LI" or "LT" or "LU" or
            "MK" or "MT" or "MD" or "MC" or "ME" or "NL" or "NO" or "PL" or "PT" or "RO" or
            "SM" or "RS" or "SK" or "SI" or "ES" or "SE" or "CH" or "TR" or "UA" or "GB" or "VA"
                => EuropeIbanValidator.ValidateIban(iban),
            "RU" => RussiaIbanValidator.ValidateIban(iban),

            // ===== MIDDLE EAST =====
            "BH" => BahrainIbanValidator.ValidateBahrainIban(iban),
            "IL" => IsraelIbanValidator.ValidateIsraelIban(iban),
            "IQ" => IraqIbanValidator.ValidateIban(iban),
            "JO" => JordanIbanValidator.ValidateJordanIban(iban),
            "KW" => KuwaitIbanValidator.ValidateKuwaitIban(iban),
            "LB" => LebanonIbanValidator.ValidateLebanonIban(iban),
            "OM" => OmanIbanValidator.ValidateIban(iban),
            "PS" => PalestineIbanValidator.ValidateIban(iban),
            "QA" => QatarIbanValidator.ValidateQatarIban(iban),
            "SA" => SaudiArabiaIbanValidator.ValidateSaudiArabiaIban(iban),
            "AE" => UAEIbanValidator.ValidateUAEIban(iban),
            "YE" => YemenIbanValidator.ValidateIban(iban),

            // ===== AFRICA =====
            "DZ" => AlgeriaIbanValidator.ValidateIban(iban),
            "AO" => AngolaIbanValidator.ValidateIban(iban),
            "BJ" => BeninIbanValidator.ValidateIban(iban),
            "BF" => BurkinaFasoIbanValidator.ValidateIban(iban),
            "BI" => BurundiIbanValidator.ValidateIban(iban),
            "CM" => CameroonIbanValidator.ValidateIban(iban),
            "CV" => CapeVerdeIbanValidator.ValidateIban(iban),
            "CF" => CentralAfricanRepublicIbanValidator.ValidateIban(iban),
            "TD" => ChadIbanValidator.ValidateIban(iban),
            "KM" => ComorosIbanValidator.ValidateIban(iban),
            "CG" => CongoIbanValidator.ValidateIban(iban),
            "CI" => CoteDIvoireIbanValidator.ValidateIban(iban),
            "DJ" => DjiboutiIbanValidator.ValidateIban(iban),
            "EG" => EgyptIbanValidator.ValidateEgyptIban(iban),
            "GQ" => EquatorialGuineaIbanValidator.ValidateIban(iban),
            "GA" => GabonIbanValidator.ValidateIban(iban),
            "GW" => GuineaBissauIbanValidator.ValidateIban(iban),
            "LY" => LibyaIbanValidator.ValidateIban(iban),
            "MA" => MoroccoIbanValidator.ValidateIban(iban),
            "MG" => MadagascarIbanValidator.ValidateIban(iban),
            "ML" => MaliIbanValidator.ValidateIban(iban),
            "MR" => MauritaniaIbanValidator.ValidateMauritaniaIban(iban),
            "MZ" => MozambiqueIbanValidator.ValidateIban(iban),
            "NE" => NigerIbanValidator.ValidateIban(iban),
            "ST" => SaoTomeAndPrincipeIbanValidator.ValidateIban(iban),
            "SN" => SenegalIbanValidator.ValidateIban(iban),
            "SC" => SeychellesIbanValidator.ValidateIban(iban),
            "SO" => SomaliaIbanValidator.ValidateIban(iban),
            "SD" => SudanIbanValidator.ValidateIban(iban),
            "TG" => TogoIbanValidator.ValidateIban(iban),
            "TN" => TunisiaIbanValidator.ValidateIban(iban),

            // ===== AMERICAS =====
            "BR" => BrazilIbanValidator.ValidateBrazilIban(iban),
            "BB" => BarbadosIbanValidator.ValidateIban(iban),
            "CR" => CostaRicaIbanValidator.ValidateCostaRicaIban(iban),
            "DO" => DominicanRepublicIbanValidator.ValidateDominicanRepublicIban(iban),
            "SV" => ElSalvadorIbanValidator.ValidateElSalvadorIban(iban),
            "GT" => GuatemalaIbanValidator.ValidateGuatemalaIban(iban),
            "HN" => HondurasIbanValidator.ValidateIban(iban),
            "NI" => NicaraguaIbanValidator.ValidateIban(iban),
            "LC" => SaintLuciaIbanValidator.ValidateIban(iban),
            "FK" => FalklandIslandsIbanValidator.ValidateIban(iban),
            "VG" => VirginIslandsBritishIbanValidator.ValidateVirginIslandsBritishIban(iban),

            // ===== ASIA =====
            "KZ" => KazakhstanIbanValidator.ValidateKazakhstanIban(iban),
            "MN" => MongoliaIbanValidator.ValidateIban(iban),
            "PK" => PakistanIbanValidator.ValidatePakistanIban(iban),
            "TL" => TimorLesteIbanValidator.ValidateTimorLesteIban(iban),

            // ===== FALLBACK =====
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountryOrInvalidIban)
        };
    }
}
