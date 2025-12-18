using Finova.Core.Common;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Mexico.Validators;

namespace Finova.Services.SouthAmerica;

public static class SouthAmericaTaxIdValidator
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
            "BR" => BrazilCnpjValidator.ValidateCnpj(taxId),
            "MX" => MexicoRfcValidator.ValidateStatic(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
