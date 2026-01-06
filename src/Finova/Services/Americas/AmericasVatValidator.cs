using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.NorthAmerica.Canada.Validators;
using Finova.Countries.NorthAmerica.CostaRica.Validators;
using Finova.Countries.NorthAmerica.DominicanRepublic.Validators;
using Finova.Countries.NorthAmerica.ElSalvador.Validators;
using Finova.Countries.NorthAmerica.Guatemala.Validators;
using Finova.Countries.NorthAmerica.Honduras.Validators;
using Finova.Countries.NorthAmerica.Nicaragua.Validators;
using Finova.Countries.SouthAmerica.Argentina.Validators;
using Finova.Countries.SouthAmerica.Brazil.Validators;
using Finova.Countries.SouthAmerica.Chile.Validators;
using Finova.Countries.SouthAmerica.Colombia.Validators;
using Finova.Countries.SouthAmerica.Mexico.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Americas VAT/GST numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
/// <example>
/// <code>
/// // Static usage
/// var result = AmericasVatValidator.ValidateVat("BR12345678901234");
///
/// // Instance usage
/// var validator = new AmericasVatValidator();
/// var result = validator.Validate("MX ABC123456AB1");
/// </code>
/// </example>
public class AmericasVatValidator : IVatValidator
{
    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public AmericasVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AmericasVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(AmericasVatValidator));
        }
        return _validators ?? Enumerable.Empty<IVatValidator>();
    }

    public string CountryCode => "";

    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
        {
            return ValidateVat(input);
        }

        string countryCode = input[0..2].ToUpperInvariant();
        var validator = GetValidators().FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        if (validator != null)
        {
            return validator.Validate(input);
        }

        return ValidateVat(input);
    }

    public VatDetails? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input) || input.Length < 2)
        {
            return GetVatDetails(input);
        }

        string countryCode = input[0..2].ToUpperInvariant();
        var validator = GetValidators().FirstOrDefault(v => v.CountryCode.Equals(countryCode, StringComparison.OrdinalIgnoreCase));

        if (validator != null)
        {
            return validator.Parse(input);
        }

        return GetVatDetails(input);
    }

    /// <summary>
    /// Validates an Americas VAT/GST number based on country code prefix.
    /// </summary>
    /// <param name="vat">The VAT number with country prefix.</param>
    /// <param name="countryCode">Optional explicit country code.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateVat(string? vat, string? countryCode = null)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (vat.Length < 2)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.VatTooShortForCountryCode);
            }
            countryCode = vat[0..2];
        }

        countryCode = countryCode.ToUpperInvariant();

        return countryCode switch
        {
            "AR" => ArgentinaVatValidator.Validate(vat),
            "BR" => BrazilVatValidator.Validate(vat),
            "CA" => CanadaGstValidator.Validate(vat),
            "CL" => ChileVatValidator.Validate(vat),
            "CO" => ColombiaVatValidator.Validate(vat),
            "MX" => MexicoVatValidator.Validate(vat),
            "CR" => CostaRicaNiteValidator.ValidateNite(vat),
            "DO" => DominicanRepublicRncValidator.ValidateRnc(vat),
            "SV" => ElSalvadorNitValidator.ValidateNit(vat),
            "GT" => GuatemalaNitValidator.ValidateNit(vat),
            "HN" => HondurasRtnValidator.ValidateRtn(vat),
            "NI" => NicaraguaRucValidator.ValidateRuc(vat),
            _ => ValidationResult.Failure(ValidationErrorCode.InvalidInput, $"Unsupported country code: {countryCode}")
        };
    }

    /// <summary>
    /// Gets details of a validated Americas VAT/GST number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat, string? countryCode = null)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(countryCode))
        {
            if (vat.Length < 2)
            {
                return null;
            }
            countryCode = vat[0..2];
        }

        countryCode = countryCode.ToUpperInvariant();

        return countryCode switch
        {
            "AR" => ArgentinaVatValidator.GetVatDetails(vat),
            "BR" => BrazilVatValidator.GetVatDetails(vat),
            "CA" => CanadaGstValidator.GetVatDetails(vat),
            "CL" => ChileVatValidator.GetVatDetails(vat),
            "CO" => ColombiaVatValidator.GetVatDetails(vat),
            "MX" => MexicoVatValidator.GetVatDetails(vat),
            "CR" => new VatDetails { VatNumber = vat!, CountryCode = "CR", IsValid = true, IdentifierKind = "NITE" },
            "DO" => new VatDetails { VatNumber = vat!, CountryCode = "DO", IsValid = true, IdentifierKind = "RNC" },
            "SV" => new VatDetails { VatNumber = vat!, CountryCode = "SV", IsValid = true, IdentifierKind = "NIT" },
            "GT" => new VatDetails { VatNumber = vat!, CountryCode = "GT", IsValid = true, IdentifierKind = "NIT" },
            "HN" => new VatDetails { VatNumber = vat!, CountryCode = "HN", IsValid = true, IdentifierKind = "RTN" },
            "NI" => new VatDetails { VatNumber = vat!, CountryCode = "NI", IsValid = true, IdentifierKind = "RUC" },
            _ => null
        };
    }
}
