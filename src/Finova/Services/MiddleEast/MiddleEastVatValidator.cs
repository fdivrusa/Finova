using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.MiddleEast.Bahrain.Validators;
using Finova.Countries.MiddleEast.Israel.Validators;
using Finova.Countries.MiddleEast.Oman.Validators;
using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using Finova.Countries.MiddleEast.UAE.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Finova.Services;

/// <summary>
/// Unified validator for Middle East VAT numbers.
/// Delegates validation to specific country validators based on the country code prefix.
/// </summary>
/// <example>
/// <code>
/// // Static usage
/// var result = MiddleEastVatValidator.ValidateVat("AE100123456789012");
///
/// // Instance usage
/// var validator = new MiddleEastVatValidator();
/// var result = validator.Validate("SA3123456789012345");
/// </code>
/// </example>
public class MiddleEastVatValidator : IVatValidator
{
    private readonly IServiceProvider? _serviceProvider;
    private IEnumerable<IVatValidator>? _validators;

    public MiddleEastVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public MiddleEastVatValidator()
    {
        _serviceProvider = null;
    }

    private IEnumerable<IVatValidator> GetValidators()
    {
        if (_validators == null && _serviceProvider != null)
        {
            _validators = _serviceProvider.GetServices<IVatValidator>()
                                          .Where(v => v.GetType() != typeof(MiddleEastVatValidator));
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
    /// Validates a Middle Eastern VAT number based on country code prefix.
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
            "AE" => UaeVatValidator.Validate(vat),
            "BH" => BahrainVatValidator.ValidateVat(vat),
            "IL" => IsraelVatValidator.Validate(vat),
            "OM" => OmanVatValidator.ValidateVat(vat),
            "SA" => SaudiArabiaVatValidator.Validate(vat),
            _ => ValidationResult.Failure(ValidationErrorCode.InvalidInput, $"Unsupported country code: {countryCode}")
        };
    }

    /// <summary>
    /// Gets details of a validated Middle Eastern VAT number.
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
            "AE" => UaeVatValidator.GetVatDetails(vat),
            "BH" => new VatDetails { VatNumber = vat!, CountryCode = "BH", IsValid = true, IdentifierKind = "VAT" },
            "IL" => IsraelVatValidator.GetVatDetails(vat),
            "OM" => new VatDetails { VatNumber = vat!, CountryCode = "OM", IsValid = true, IdentifierKind = "VAT" },
            "SA" => SaudiArabiaVatValidator.GetVatDetails(vat),
            _ => null
        };
    }
}
