using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Asia.China.Validators;
using Finova.Countries.Asia.India.Validators;
using Finova.Countries.Asia.Japan.Validators;
using Finova.Countries.Asia.Singapore.Validators;
using Finova.Countries.Asia.SouthKorea.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Asian VAT/GST numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
/// <example>
/// <code>
/// // Static usage
/// var result = AsiaVatValidator.ValidateVat("IN22AAAAA0000A1Z5");
///
/// // Instance usage
/// var validator = new AsiaVatValidator();
/// var result = validator.Validate("SG123456789A");
/// </code>
/// </example>
public class AsiaVatValidator : IVatValidator
{
    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public AsiaVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public AsiaVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(AsiaVatValidator));
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
    /// Validates an Asian VAT/GST number based on country code prefix.
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
            "CN" => ChinaVatValidator.Validate(vat),
            "IN" => IndiaGstinValidator.Validate(vat),
            "JP" => JapanVatValidator.Validate(vat),
            "KR" => SouthKoreaVatValidator.Validate(vat),
            "SG" => SingaporeGstValidator.Validate(vat),
            _ => ValidationResult.Failure(ValidationErrorCode.InvalidInput, $"Unsupported country code: {countryCode}")
        };
    }

    /// <summary>
    /// Gets details of a validated Asian VAT/GST number.
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
            "CN" => ChinaVatValidator.GetVatDetails(vat),
            "IN" => IndiaGstinValidator.GetVatDetails(vat),
            "JP" => JapanVatValidator.GetVatDetails(vat),
            "KR" => SouthKoreaVatValidator.GetVatDetails(vat),
            "SG" => SingaporeGstValidator.GetVatDetails(vat),
            _ => null
        };
    }
}
