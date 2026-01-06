using Finova.Core.Common;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using Finova.Countries.NorthAmerica.Honduras.Validators;
using Finova.Countries.NorthAmerica.Nicaragua.Validators;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;

namespace Finova.Services.NorthAmerica;

public static class NorthAmericaTaxIdValidator
{
    public static ValidationResult Validate(string? taxId, string? countryCode = null)
    {
        if (string.IsNullOrWhiteSpace(taxId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (taxId.Length < 2)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.VatTooShortForCountryCode);
            }
            countryCode = taxId[0..2];
        }

        return countryCode.ToUpperInvariant() switch
        {
            "CA" => new CanadaBusinessNumberValidator().Validate(taxId),
            "US" => new UnitedStatesEinValidator().Validate(taxId),
            "CR" => new CostaRicaNiteValidator().Validate(taxId),
            "DO" => new DominicanRepublicRncValidator().Validate(taxId),
            "SV" => new ElSalvadorNitValidator().Validate(taxId),
            "GT" => new GuatemalaNitValidator().Validate(taxId),
            "HN" => new HondurasRtnValidator().Validate(taxId),
            "NI" => new NicaraguaRucValidator().Validate(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
