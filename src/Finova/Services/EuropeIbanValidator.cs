
using Finova.Core.Iban;
using Finova.Countries.Europe.Andorra.Validators;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Countries.Europe.Croatia.Validators;
using Finova.Countries.Europe.Cyprus.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Estonia.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
using Finova.Countries.Europe.Gibraltar.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Latvia.Validators;
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Monaco.Validators;
using Finova.Countries.Europe.Netherlands.Validators;
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Poland.Validators;
using Finova.Countries.Europe.Portugal.Validators;
using Finova.Countries.Europe.Romania.Validators;
using Finova.Countries.Europe.SanMarino.Validators;
using Finova.Countries.Europe.Slovakia.Validators;
using Finova.Countries.Europe.Slovenia.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.Switzerland.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Countries.Europe.Vatican.Validators;

using Finova.Core.Common;

namespace Finova.Services;

public class EuropeIbanValidator : IIbanValidator
{
    public string CountryCode => "";

    public ValidationResult Validate(string? iban) => ValidateIban(iban);

    public static ValidationResult ValidateIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        if (iban.Length < 2)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN is too short.");
        }

        string country = iban[0..2].ToUpperInvariant();

        return country switch
        {
            // Route to specific static methods
            "DE" => GermanyIbanValidator.ValidateGermanyIban(iban),
            "IT" => ItalyIbanValidator.ValidateItalyIban(iban),
            "ES" => SpainIbanValidator.ValidateSpainIban(iban),
            "FR" => FranceIbanValidator.ValidateFranceIban(iban),
            "BE" => BelgiumIbanValidator.ValidateBelgiumIban(iban),
            "NL" => NetherlandsIbanValidator.ValidateNetherlandsIban(iban),
            "GB" => UnitedKingdomIbanValidator.ValidateUnitedKingdomIban(iban),
            "LU" => LuxembourgIbanValidator.ValidateLuxembourgIban(iban),
            "IE" => IrelandIbanValidator.ValidateIrelandIban(iban),
            "AT" => AustriaIbanValidator.ValidateAustriaIban(iban),
            "GR" => GreeceIbanValidator.ValidateGreeceIban(iban),
            "FI" => FinlandIbanValidator.ValidateFinlandIban(iban),
            "PT" => PortugalIbanValidator.ValidatePortugalIban(iban),
            "SE" => SwedenIbanValidator.ValidateSwedenIban(iban),
            "DK" => DenmarkIbanValidator.ValidateDenmarkIban(iban),
            "NO" => NorwayIbanValidator.ValidateNorwayIban(iban),
            "PL" => PolandIbanValidator.ValidatePolandIban(iban),
            "CZ" => CzechRepublicIbanValidator.ValidateCzechIban(iban),
            "HU" => HungaryIbanValidator.ValidateHungaryIban(iban),
            "RO" => RomaniaIbanValidator.ValidateRomaniaIban(iban),
            "BG" => BulgariaIbanValidator.ValidateBulgariaIban(iban),
            "HR" => CroatiaIbanValidator.ValidateCroatiaIban(iban),
            "SI" => SloveniaIbanValidator.ValidateSloveniaIban(iban),
            "SK" => SlovakiaIbanValidator.ValidateSlovakiaIban(iban),
            "EE" => EstoniaIbanValidator.ValidateEstoniaIban(iban),
            "LV" => LatviaIbanValidator.ValidateLatviaIban(iban),
            "LT" => LithuaniaIbanValidator.ValidateLithuaniaIban(iban),
            "CY" => CyprusIbanValidator.ValidateCyprusIban(iban),
            "MT" => MaltaIbanValidator.ValidateMaltaIban(iban),
            "CH" => SwitzerlandIbanValidator.ValidateSwitzerlandIban(iban),
            "MC" => MonacoIbanValidator.ValidateMonacoIban(iban),
            "AD" => AndorraIbanValidator.ValidateAndorraIban(iban),
            "VA" => VaticanIbanValidator.ValidateVaticanIban(iban),
            "SM" => SanMarinoIbanValidator.ValidateSanMarinoIban(iban),
            "GI" => GibraltarIbanValidator.ValidateGibraltarIban(iban),
            "IS" => IcelandIbanValidator.ValidateIcelandIban(iban),

            _ => IbanHelper.IsValidIban(iban)
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Country code {country} is not supported or IBAN is invalid.")
        };
    }
}
