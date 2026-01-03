using Finova.Core.Iban;
using Finova.Core.Common;

// Middle East
using Finova.Countries.MiddleEast.Bahrain.Validators;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.Jordan.Validators;
using Finova.Countries.MiddleEast.Kuwait.Validators;
using Finova.Countries.MiddleEast.Lebanon.Validators;
using Finova.Countries.MiddleEast.Qatar.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;

// Africa
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Mauritania.Validators;

// Americas
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using Finova.Countries.NorthAmerica.VirginIslandsBritish.Validators;

// Asia
using Finova.Countries.Asia.Kazakhstan.Validators;
using Finova.Countries.Asia.Pakistan.Validators;
using Finova.Countries.Asia.TimorLeste.Validators;

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

            // ===== MIDDLE EAST =====
            "BH" => BahrainIbanValidator.ValidateBahrainIban(iban),
            "IL" => IsraelIbanValidator.ValidateIsraelIban(iban),
            "JO" => JordanIbanValidator.ValidateJordanIban(iban),
            "KW" => KuwaitIbanValidator.ValidateKuwaitIban(iban),
            "LB" => LebanonIbanValidator.ValidateLebanonIban(iban),
            "QA" => QatarIbanValidator.ValidateQatarIban(iban),
            "SA" => SaudiArabiaIbanValidator.ValidateSaudiArabiaIban(iban),
            "AE" => UAEIbanValidator.ValidateUAEIban(iban),

            // ===== AFRICA =====
            "EG" => EgyptIbanValidator.ValidateEgyptIban(iban),
            "MR" => MauritaniaIbanValidator.ValidateMauritaniaIban(iban),

            // ===== AMERICAS =====
            "BR" => BrazilIbanValidator.ValidateBrazilIban(iban),
            "CR" => CostaRicaIbanValidator.ValidateCostaRicaIban(iban),
            "DO" => DominicanRepublicIbanValidator.ValidateDominicanRepublicIban(iban),
            "SV" => ElSalvadorIbanValidator.ValidateElSalvadorIban(iban),
            "GT" => GuatemalaIbanValidator.ValidateGuatemalaIban(iban),
            "VG" => VirginIslandsBritishIbanValidator.ValidateVirginIslandsBritishIban(iban),

            // ===== ASIA =====
            "KZ" => KazakhstanIbanValidator.ValidateKazakhstanIban(iban),
            "PK" => PakistanIbanValidator.ValidatePakistanIban(iban),
            "TL" => TimorLesteIbanValidator.ValidateTimorLesteIban(iban),

            // ===== FALLBACK =====
            _ => IbanHelper.IsValidIban(iban)
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountryOrInvalidIban)
        };
    }
}
