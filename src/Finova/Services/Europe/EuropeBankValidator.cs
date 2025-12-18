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
/// Unified validator for European Bank Routing and Account Numbers.
/// </summary>
public class EuropeBankValidator
{
    /// <summary>
    /// Validates a Bank Routing Number for the specified European country.
    /// </summary>
    public static ValidationResult ValidateRoutingNumber(string countryCode, string? routingNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "DE" => new GermanyBankleitzahlValidator().Validate(routingNumber),
            "FR" => new FranceBankCodeValidator().Validate(routingNumber),
            "IT" => new ItalyBankCodeValidator().Validate(routingNumber),
            "ES" => new SpainBankCodeValidator().Validate(routingNumber),
            "GB" => UnitedKingdomSortCodeValidator.ValidateStatic(routingNumber),
            "UK" => UnitedKingdomSortCodeValidator.ValidateStatic(routingNumber), // Alias
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Bank Account Number for the specified European country.
    /// </summary>
    public static ValidationResult ValidateBankAccount(string countryCode, string? accountNumber)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "GB" => new UnitedKingdomBankAccountValidator().Validate(accountNumber),
            "UK" => new UnitedKingdomBankAccountValidator().Validate(accountNumber), // Alias
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a BBAN (Basic Bank Account Number) for the specified European country.
    /// </summary>
    public static ValidationResult ValidateBban(string countryCode, string? bban)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "AL" => AlbaniaBbanValidator.Validate(bban),
            "AD" => AndorraBbanValidator.Validate(bban),
            "AT" => AustriaBbanValidator.Validate(bban),
            "AZ" => AzerbaijanBbanValidator.Validate(bban),
            "BY" => BelarusBbanValidator.Validate(bban),
            "BE" => BelgiumBbanValidator.Validate(bban),
            "BA" => BosniaAndHerzegovinaBbanValidator.Validate(bban),
            "BG" => BulgariaBbanValidator.Validate(bban),
            "HR" => CroatiaBbanValidator.Validate(bban),
            "CY" => CyprusBbanValidator.Validate(bban),
            "CZ" => CzechRepublicBbanValidator.Validate(bban),
            "DK" => DenmarkBbanValidator.Validate(bban),
            "EE" => EstoniaBbanValidator.Validate(bban),
            "FO" => FaroeIslandsBbanValidator.Validate(bban),
            "FI" => FinlandBbanValidator.Validate(bban),
            "FR" => FranceBbanValidator.Validate(bban),
            "GE" => GeorgiaBbanValidator.Validate(bban),
            "DE" => GermanyBbanValidator.Validate(bban),
            "GI" => GibraltarBbanValidator.Validate(bban),
            "GR" => GreeceBbanValidator.Validate(bban),
            "GL" => GreenlandBbanValidator.Validate(bban),
            "HU" => HungaryBbanValidator.Validate(bban),
            "IS" => IcelandBbanValidator.Validate(bban),
            "IE" => IrelandBbanValidator.Validate(bban),
            "IT" => ItalyBbanValidator.Validate(bban),
            "XK" => KosovoBbanValidator.Validate(bban),
            "LV" => LatviaBbanValidator.Validate(bban),
            "LI" => LiechtensteinBbanValidator.Validate(bban),
            "LT" => LithuaniaBbanValidator.Validate(bban),
            "LU" => LuxembourgBbanValidator.Validate(bban),
            "MT" => MaltaBbanValidator.Validate(bban),
            "MD" => MoldovaBbanValidator.Validate(bban),
            "MC" => MonacoBbanValidator.Validate(bban),
            "ME" => MontenegroBbanValidator.Validate(bban),
            "NL" => NetherlandsBbanValidator.Validate(bban),
            "MK" => NorthMacedoniaBbanValidator.Validate(bban),
            "NO" => NorwayBbanValidator.Validate(bban),
            "PL" => PolandBbanValidator.Validate(bban),
            "PT" => PortugalBbanValidator.Validate(bban),
            "RO" => RomaniaBbanValidator.Validate(bban),
            "SM" => SanMarinoBbanValidator.Validate(bban),
            "RS" => SerbiaBbanValidator.Validate(bban),
            "SK" => SlovakiaBbanValidator.Validate(bban),
            "SI" => SloveniaBbanValidator.Validate(bban),
            "ES" => SpainBbanValidator.Validate(bban),
            "SE" => SwedenBbanValidator.Validate(bban),
            "CH" => SwitzerlandBbanValidator.Validate(bban),
            "TR" => TurkeyBbanValidator.Validate(bban),
            "UA" => UkraineBbanValidator.Validate(bban),
            "GB" => UnitedKingdomBbanValidator.Validate(bban),
            "UK" => UnitedKingdomBbanValidator.Validate(bban), // Alias
            "VA" => VaticanBbanValidator.Validate(bban),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Instance method for validating routing numbers (DI friendly).
    /// </summary>
    public ValidationResult ValidateRouting(string countryCode, string? routingNumber)
    {
        return ValidateRoutingNumber(countryCode, routingNumber);
    }

    /// <summary>
    /// Instance method for validating account numbers (DI friendly).
    /// </summary>
    public ValidationResult ValidateAccount(string countryCode, string? accountNumber)
    {
        return ValidateBankAccount(countryCode, accountNumber);
    }

    /// <summary>
    /// Instance method for validating BBAN (DI friendly).
    /// </summary>
    public ValidationResult ValidateBbanInstance(string countryCode, string? bban)
    {
        return ValidateBban(countryCode, bban);
    }
}
