using Finova.Core.Common;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
using Finova.Countries.Europe.Germany.Validators;
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
using Finova.Countries.Europe.Norway.Validators;
using Finova.Countries.Europe.Spain.Validators;
using Finova.Countries.Europe.Sweden.Validators;
using Finova.Countries.Europe.UnitedKingdom.Validators;

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
            "DK" => DenmarkCprValidator.ValidateStatic(nationalId),
            "FI" => FinlandHenkilotunnusValidator.ValidateStatic(nationalId),
            "FR" => FranceNationalIdValidator.ValidateStatic(nationalId),
            "DE" => GermanyNationalIdValidator.ValidateStatic(nationalId),
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
            "NO" => NorwayFodselsnummerValidator.ValidateStatic(nationalId),
            "ES" => SpainNationalIdValidator.ValidateStatic(nationalId),
            "SE" => SwedenPersonnummerValidator.ValidateStatic(nationalId),
            "GB" or "UK" => UnitedKingdomNinoValidator.ValidateStatic(nationalId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
