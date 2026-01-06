using Finova.Core.Common;
using Finova.Countries.Africa.Egypt.Validators;
using Finova.Countries.Africa.Kenya.Validators;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.UnitedStates.Validators;
using Finova.Countries.Oceania.Australia.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Argentina.Validators;
using Finova.Countries.SouthAmerica.Chile.Validators;
using Finova.Countries.SouthAmerica.Colombia.Validators;
using Finova.Countries.SouthAmerica.Mexico.Validators;
using Finova.Countries.SoutheastAsia.Indonesia.Validators;
using Finova.Countries.SoutheastAsia.Malaysia.Validators;
using Finova.Countries.SoutheastAsia.Thailand.Validators;
using Finova.Countries.SoutheastAsia.Vietnam.Validators;

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
            "EG" => EgyptNationalIdValidator.ValidateStatic(nationalId),
            "KE" => KenyaNationalIdValidator.ValidateStatic(nationalId),
            "ID" => IndonesiaNikValidator.ValidateStatic(nationalId),
            "MY" => MalaysiaMyKadValidator.ValidateStatic(nationalId),
            "TH" => ThailandIdValidator.ValidateStatic(nationalId),
            "VN" => VietnamCitizenIdValidator.ValidateStatic(nationalId),
            "AR" => ArgentinaCuitValidator.ValidateStatic(nationalId),
            "CL" => ChileRutValidator.ValidateStatic(nationalId),
            "CO" => ColombiaCedulaValidator.ValidateStatic(nationalId),
            "MX" => MexicoCurpValidator.ValidateStatic(nationalId),
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
            "CN" => ChinaUnifiedSocialCreditCodeValidator.ValidateUscc(taxId),
            "IN" => new IndiaPanValidator().Validate(taxId),
            "CA" => new CanadaBusinessNumberValidator().Validate(taxId),
            "US" => new UnitedStatesEinValidator().Validate(taxId),
            "AU" => ValidateAustraliaTaxId(taxId),
            "BR" => new BrazilCnpjValidator().Validate(taxId),
            "AR" => ArgentinaCuitValidator.ValidateStatic(taxId),
            "CL" => ChileRutValidator.ValidateStatic(taxId),
            "CO" => ColombiaVatValidator.Validate(taxId),
            "MX" => MexicoRfcValidator.ValidateStatic(taxId),
            "KZ" => new Finova.Countries.Asia.Kazakhstan.Validators.KazakhstanBinValidator().Validate(taxId),
            "VN" => new VietnamTaxIdValidator().Validate(taxId),
            "EG" => new EgyptTaxRegistrationNumberValidator().Validate(taxId),
            "MA" => new Finova.Countries.Africa.Morocco.Validators.MoroccoIceValidator().Validate(taxId),
            "DZ" => new Finova.Countries.Africa.Algeria.Validators.AlgeriaNifValidator().Validate(taxId),
            "TN" => new Finova.Countries.Africa.Tunisia.Validators.TunisiaMatriculeFiscalValidator().Validate(taxId),
            "NG" => new Finova.Countries.Africa.Nigeria.Validators.NigeriaTinValidator().Validate(taxId),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, ValidationMessages.UnsupportedCountry)
        };
    }

    private static ValidationResult ValidateAustraliaTaxId(string? taxId)
    {
        var tfnResult = new AustraliaTfnValidator().Validate(taxId);
        if (tfnResult.IsValid)
        {
            return tfnResult;
        }

        var abnResult = new AustraliaAbnValidator().Validate(taxId);
        if (abnResult.IsValid)
        {
            return abnResult;
        }

        return tfnResult;
    }
}