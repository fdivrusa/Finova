using Finova.Core.Common;
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
using Finova.Countries.Europe.Gibraltar.Validators;
using Finova.Countries.Europe.Greece.Validators;
using Finova.Countries.Europe.Greenland.Validators;
using Finova.Countries.Europe.Hungary.Validators;
using Finova.Countries.Europe.Iceland.Validators;
using Finova.Countries.Europe.Ireland.Validators;
using Finova.Countries.Europe.Italy.Validators;
using Finova.Countries.Europe.Kosovo.Validators;
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
using Finova.Countries.Europe.Vatican.Validators;

namespace Finova.Services;

/// <summary>
/// Static facade for validating National IDs across Europe.
/// </summary>
public static class EuropeNationalIdValidator
{
    /// <summary>
    /// Validates a National ID for the specified European country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="nationalId">The National ID to validate.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult Validate(string countryCode, string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "AL" => new AlbaniaNationalIdValidator().Validate(nationalId),
            "AD" => new AndorraNationalIdValidator().Validate(nationalId),
            "AT" => new AustriaNationalIdValidator().Validate(nationalId),
            "AZ" => new AzerbaijanNationalIdValidator().Validate(nationalId),
            "BY" => new BelarusNationalIdValidator().Validate(nationalId),
            "BE" => BelgiumNationalIdValidator.ValidateStatic(nationalId),
            "BA" => new BosniaAndHerzegovinaNationalIdValidator().Validate(nationalId),
            "BG" => new BulgariaNationalIdValidator().Validate(nationalId),
            "HR" => CroatiaOibValidator.ValidateOib(nationalId), // Croatia uses OIB for both
            "CY" => new CyprusNationalIdValidator().Validate(nationalId),
            "CZ" => new CzechRepublicNationalIdValidator().Validate(nationalId),
            "DK" => DenmarkCprValidator.ValidateStatic(nationalId),
            "EE" => new EstoniaNationalIdValidator().Validate(nationalId),
            "FO" => new FaroeIslandsNationalIdValidator().Validate(nationalId),
            "FI" => FinlandHenkilotunnusValidator.ValidateStatic(nationalId),
            "FR" => FranceNationalIdValidator.ValidateStatic(nationalId),
            "GE" => new GeorgiaNationalIdValidator().Validate(nationalId),
            "DE" => GermanyNationalIdValidator.ValidateStatic(nationalId),
            "GI" => new GibraltarNationalIdValidator().Validate(nationalId),
            "GR" => new GreeceNationalIdValidator().Validate(nationalId),
            "GL" => GreenlandNationalIdValidator.ValidateStatic(nationalId),
            "HU" => HungaryNationalIdValidator.ValidateStatic(nationalId),
            "IS" => IcelandKennitalaValidator.ValidateStatic(nationalId),
            "IE" => IrelandNationalIdValidator.ValidateStatic(nationalId),
            "IT" => ItalyNationalIdValidator.ValidateStatic(nationalId),
            "XK" => KosovoNationalIdValidator.ValidateStatic(nationalId),
            "LV" => LatviaNationalIdValidator.ValidateStatic(nationalId),
            "LI" => LiechtensteinNationalIdValidator.ValidateStatic(nationalId),
            "LT" => LithuaniaNationalIdValidator.ValidateStatic(nationalId),
            "LU" => LuxembourgNationalIdValidator.ValidateStatic(nationalId),
            "MT" => MaltaNationalIdValidator.ValidateStatic(nationalId),
            "MD" => MoldovaNationalIdValidator.ValidateStatic(nationalId),
            "MC" => new MonacoNationalIdValidator().Validate(nationalId),
            "ME" => new MontenegroNationalIdValidator().Validate(nationalId),
            "NL" => NetherlandsNationalIdValidator.ValidateStatic(nationalId),
            "MK" => new NorthMacedoniaNationalIdValidator().Validate(nationalId),
            "NO" => NorwayNationalIdValidator.ValidateStatic(nationalId),
            "PL" => new PolandNationalIdValidator().Validate(nationalId),
            "PT" => new PortugalNationalIdValidator().Validate(nationalId),
            "RO" => new RomaniaNationalIdValidator().Validate(nationalId),
            "SM" => new SanMarinoNationalIdValidator().Validate(nationalId),
            "RS" => new SerbiaNationalIdValidator().Validate(nationalId),
            "SK" => new SlovakiaNationalIdValidator().Validate(nationalId),
            "SI" => new SloveniaNationalIdValidator().Validate(nationalId),
            "ES" => SpainNationalIdValidator.ValidateStatic(nationalId),
            "SE" => SwedenNationalIdValidator.ValidateStatic(nationalId),
            "CH" => new SwitzerlandNationalIdValidator().Validate(nationalId),
            "TR" => new TurkeyNationalIdValidator().Validate(nationalId),
            "UA" => new UkraineNationalIdValidator().Validate(nationalId),
            "GB" or "UK" => UnitedKingdomNationalIdValidator.ValidateStatic(nationalId),
            "VA" => new VaticanNationalIdValidator().Validate(nationalId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}