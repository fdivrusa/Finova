using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Services;

/// <summary>
/// Unified global validator for VAT/GST numbers across all supported regions.
/// </summary>
public class GlobalVatValidator : IVatValidator
{
    private readonly IServiceProvider? _serviceProvider;

    public GlobalVatValidator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public GlobalVatValidator()
    {
        _serviceProvider = null;
    }

    public string CountryCode => "";

    /// <summary>
    /// Validates a VAT number globally.
    /// </summary>
    /// <param name="input">The VAT number (usually with country prefix).</param>
    /// <returns>A ValidationResult.</returns>
    public ValidationResult Validate(string? input)
    {
        return ValidateVat(input);
    }

    /// <summary>
    /// Parses a VAT number globally.
    /// </summary>
    /// <param name="input">The VAT number.</param>
    /// <returns>VatDetails if valid, null otherwise.</returns>
    public VatDetails? Parse(string? input)
    {
        return GetVatDetails(input);
    }

    /// <summary>
    /// Static validation method that routes to regional validators.
    /// </summary>
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
            // Europe
            "AL" or "AD" or "AT" or "AZ" or "BA" or "BE" or "BG" or "BY" or "CH" or "CHE" or "CY" or "CZ" or "DE" or "DK" or "EE" or "EL" or "GR" or "ES" or "FI" or "FO" or "FR" or "GB" or "GE" or "HR" or "HU" or "IE" or "IS" or "IT" or "XK" or "LI" or "LT" or "LU" or "LV" or "MC" or "MD" or "ME" or "MK" or "MT" or "NL" or "NO" or "PL" or "PT" or "RO" or "RS" or "SE" or "SI" or "SK" or "SM" or "TR" or "UA" or "RU"
                => EuropeVatValidator.ValidateVat(vat, countryCode),

            // Middle East
            "AE" or "BH" or "IL" or "OM" or "SA"
                => MiddleEastVatValidator.ValidateVat(vat, countryCode),

            // Africa
            "AO" or "CI" or "DZ" or "EG" or "MA" or "NG" or "SN" or "TN" or "ZA"
                => AfricaVatValidator.ValidateVat(vat, countryCode),

            // Asia
            "CN" or "IN" or "JP" or "KR" or "SG" or "VN" or "KZ"
                => AsiaVatValidator.ValidateVat(vat, countryCode),

            // Americas
            "AR" or "BR" or "CA" or "CL" or "CO" or "MX" or "CR" or "DO" or "SV" or "GT" or "HN" or "NI"
                => AmericasVatValidator.ValidateVat(vat, countryCode),

            // Oceania
            "AU" or "NZ"
                => OceaniaVatValidator.ValidateVat(vat, countryCode),

            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Unsupported country code: {countryCode}")
        };
    }

    /// <summary>
    /// Static method to get VAT details globally.
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
            // Europe
            "AL" or "AD" or "AT" or "AZ" or "BA" or "BE" or "BG" or "BY" or "CH" or "CHE" or "CY" or "CZ" or "DE" or "DK" or "EE" or "EL" or "GR" or "ES" or "FI" or "FO" or "FR" or "GB" or "GE" or "HR" or "HU" or "IE" or "IS" or "IT" or "XK" or "LI" or "LT" or "LU" or "LV" or "MC" or "MD" or "ME" or "MK" or "MT" or "NL" or "NO" or "PL" or "PT" or "RO" or "RS" or "SE" or "SI" or "SK" or "SM" or "TR" or "UA" or "RU"
                => EuropeVatValidator.GetVatDetails(vat, countryCode),

            // Middle East
            "AE" or "BH" or "IL" or "OM" or "SA"
                => MiddleEastVatValidator.GetVatDetails(vat, countryCode),

            // Africa
            "AO" or "CI" or "DZ" or "EG" or "MA" or "NG" or "SN" or "TN" or "ZA"
                => AfricaVatValidator.GetVatDetails(vat, countryCode),

            // Asia
            "CN" or "IN" or "JP" or "KR" or "SG" or "VN" or "KZ"
                => AsiaVatValidator.GetVatDetails(vat, countryCode),

            // Americas
            "AR" or "BR" or "CA" or "CL" or "CO" or "MX" or "CR" or "DO" or "SV" or "GT" or "HN" or "NI"
                => AmericasVatValidator.GetVatDetails(vat, countryCode),

            // Oceania
            "AU" or "NZ"
                => OceaniaVatValidator.GetVatDetails(vat, countryCode),

            _ => null
        };
    }
}
