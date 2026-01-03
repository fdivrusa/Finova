using Finova.Core.Iban;
using Finova.Countries.Europe.Andorra.Validators;
using Finova.Countries.Europe.Austria.Validators;
using Finova.Countries.Europe.Azerbaijan.Validators;
using Finova.Countries.Europe.Belarus.Validators;
using Finova.Countries.Europe.Belgium.Validators;
using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Countries.Europe.Croatia.Validators;
using Finova.Countries.Europe.Cyprus.Validators;
using Finova.Countries.Europe.CzechRepublic.Validators;
using Finova.Countries.Europe.Denmark.Validators;
using Finova.Countries.Europe.Estonia.Validators;
using Finova.Countries.Europe.FaroeIslands.Validators;
using Finova.Countries.Europe.Finland.Validators;
using Finova.Countries.Europe.France.Validators;
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
using Finova.Countries.Europe.Lithuania.Validators;
using Finova.Countries.Europe.Luxembourg.Validators;
using Finova.Countries.Europe.Malta.Validators;
using Finova.Countries.Europe.Moldova.Validators;
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
using Finova.Countries.Europe.Liechtenstein.Validators;
using Finova.Countries.Europe.Serbia.Validators;
using Finova.Countries.Europe.Ukraine.Validators;
using Finova.Countries.Europe.Montenegro.Validators;
using Finova.Countries.Europe.Albania.Validators;
using Finova.Countries.Europe.Turkey.Validators;
using Finova.Countries.Europe.NorthMacedonia.Validators;
using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using Finova.Countries.Europe.Georgia.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Finova.Services;

/// <summary>
/// Unified validator for European IBANs.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
/// <example>
/// <code>
/// // Static usage
/// var result = EuropeIbanValidator.ValidateIban("BE68539007547034");
///
/// // Instance usage (DI)
/// var validator = new EuropeIbanValidator();
/// var result = validator.Validate("FR7630006000011234567890189");
/// </code>
/// </example>
public class EuropeIbanValidator : IIbanValidator
{
    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IIbanValidator>? _validators;

    public EuropeIbanValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public EuropeIbanValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IIbanValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IIbanValidator>()
                                          .Where(v => v.GetType() != typeof(EuropeIbanValidator));
        }
        return _validators ?? [];
    }

    public string CountryCode => "";

    public ValidationResult Validate(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban) || iban.Length < 2)
        {
            return ValidateIban(iban); // Fallback to static validation for basic checks
        }

        string countryCode = iban[0..2].ToUpperInvariant();
        var validator = GetValidators().FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        if (validator != null)
        {
            return validator.Validate(iban);
        }

        // Fallback to static logic if no specific validator is found in DI
        return ValidateIban(iban);
    }

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
            "LI" => LiechtensteinIbanValidator.ValidateLiechtensteinIban(iban),
            "RS" => SerbiaIbanValidator.ValidateSerbiaIban(iban),
            "UA" => UkraineIbanValidator.ValidateUkraineIban(iban),
            "ME" => MontenegroIbanValidator.ValidateMontenegroIban(iban),
            "AL" => AlbaniaIbanValidator.ValidateAlbaniaIban(iban),
            "TR" => TurkeyIbanValidator.ValidateTurkeyIban(iban),
            "MK" => NorthMacedoniaIbanValidator.ValidateNorthMacedoniaIban(iban),
            "BA" => BosniaAndHerzegovinaIbanValidator.ValidateBosniaAndHerzegovinaIban(iban),
            "GE" => GeorgiaIbanValidator.ValidateGeorgiaIban(iban),
            // Additional European territories and countries
            "FO" => FaroeIslandsIbanValidator.ValidateFaroeIslandsIban(iban),
            "GL" => GreenlandIbanValidator.ValidateGreenlandIban(iban),
            "XK" => KosovoIbanValidator.ValidateKosovoIban(iban),
            "MD" => MoldovaIbanValidator.ValidateMoldovaIban(iban),
            "BY" => BelarusIbanValidator.ValidateBelarusIban(iban),
            "AZ" => AzerbaijanIbanValidator.ValidateAzerbaijanIban(iban),

            _ => IbanHelper.IsValidIban(iban)
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountryOrInvalidIban)
        };
    }
}
