using Finova.Core.Common;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.Oceania.Australia.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;

namespace Finova.Services;

/// <summary>
/// Unified validator for Global Identity Numbers (National ID, Tax ID).
/// Delegates validation to specific country validators.
/// </summary>
public static class GlobalIdentityValidator
{
    /// <summary>
    /// Validates a National ID for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="nationalId">The National ID to validate.</param>
    public static ValidationResult ValidateNationalId(string countryCode, string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "CN" => new ChinaResidentIdentityCardValidator().Validate(nationalId),
            "IN" => new IndiaAadhaarValidator().Validate(nationalId),
            "CA" => new CanadaSinValidator().Validate(nationalId),
            "BR" => new BrazilCpfValidator().Validate(nationalId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    /// <summary>
    /// Validates a Tax ID for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    /// <param name="taxId">The Tax ID to validate.</param>
    public static ValidationResult ValidateTaxId(string countryCode, string? taxId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return countryCode.ToUpperInvariant() switch
        {
            "CN" => new ChinaUnifiedSocialCreditCodeValidator().Validate(taxId),
            "IN" => new IndiaPanValidator().Validate(taxId),
            "CA" => new CanadaBusinessNumberValidator().Validate(taxId),
            "US" => new UnitedStatesEinValidator().Validate(taxId),
            "AU" => ValidateAustraliaTaxId(taxId),
            "BR" => new BrazilCnpjValidator().Validate(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    private static ValidationResult ValidateAustraliaTaxId(string? taxId)
    {
        var tfnResult = new AustraliaTfnValidator().Validate(taxId);
        if (tfnResult.IsValid) return tfnResult;

        var abnResult = new AustraliaAbnValidator().Validate(taxId);
        if (abnResult.IsValid) return abnResult;

        return tfnResult;
    }
}
