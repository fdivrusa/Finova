using Finova.Core.Common;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Singapore.Validators;

namespace Finova.Services.Asia;

public static class AsiaTaxIdValidator
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
            "CN" => ChinaUnifiedSocialCreditCodeValidator.ValidateUscc(taxId),
            "JP" => JapanCorporateNumberValidator.ValidateStatic(taxId),
            "SG" => SingaporeUenValidator.ValidateStatic(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }
}
