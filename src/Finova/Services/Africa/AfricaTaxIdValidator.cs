using Finova.Core.Common;
using Finova.Countries.Africa.Algeria.Validators;
using Finova.Countries.Africa.Angola.Validators;
using Finova.Countries.Africa.CoteDIvoire.Validators;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Morocco.Validators;
using Finova.Countries.Africa.Nigeria.Validators;
using Finova.Countries.Africa.Senegal.Validators;
using Finova.Countries.Africa.Tunisia.Validators;

namespace Finova.Services.Africa;

public static class AfricaTaxIdValidator
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
            "AO" => new AngolaNifValidator().Validate(taxId),
            "CI" => new IvoryCoastNccValidator().Validate(taxId),
            "DZ" => new AlgeriaNifValidator().Validate(taxId),
            "EG" => new EgyptTaxRegistrationNumberValidator().Validate(taxId),
            "MA" => new MoroccoIceValidator().Validate(taxId),
            "NG" => new NigeriaTinValidator().Validate(taxId),
            "SN" => new SenegalNineaValidator().Validate(taxId),
            "TN" => new TunisiaMatriculeFiscalValidator().Validate(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}