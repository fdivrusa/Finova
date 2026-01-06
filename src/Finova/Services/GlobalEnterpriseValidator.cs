using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Services.Africa;
using Finova.Services.Asia;
using Finova.Services.NorthAmerica;
using Finova.Services.SouthAmerica;

namespace Finova.Services;

/// <summary>
/// Unified global validator for Enterprise/Business Registration Numbers.
/// Routes validation to specific regional validators based on the country code.
/// </summary>
public class GlobalEnterpriseValidator : IGlobalEnterpriseValidator
{
    /// <inheritdoc/>
    public ValidationResult Validate(string? number, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string country = countryCode.ToUpperInvariant();

        // Check Europe (Most comprehensive enterprise validation)
        var result = EuropeEnterpriseValidator.ValidateEnterpriseNumber(number, country);
        if (result.IsValid || result.Errors.All(e => e.Code != ValidationErrorCode.UnsupportedCountry))
        {
            // If it's a supported European country, return the result
            return result;
        }

        // Route to other regions (often using Tax ID as Enterprise ID)
        return country switch
        {
            // Africa
            "DZ" or "AO" or "CI" or "EG" or "MA" or "NG" or "SN" or "TN" or "ZA"
                => AfricaTaxIdValidator.Validate(number, country),

            // Asia
            "CN" or "IN" or "JP" or "KR" or "SG" or "VN" or "KZ"
                => AsiaTaxIdValidator.Validate(number, country),

            // North America
            "US" or "CA" or "CR" or "DO" or "SV" or "GT" or "HN" or "NI"
                => NorthAmericaTaxIdValidator.Validate(number, country),

            // South America
            "AR" or "BR" or "CL" or "CO" or "MX"
                => SouthAmericaTaxIdValidator.Validate(number, country),

            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, $"Country code {countryCode} is not supported for enterprise validation.")
        };
    }

    /// <inheritdoc/>
    public string? Parse(string? number, string countryCode)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return null;
        }

        string country = countryCode.ToUpperInvariant();

        // Europe normalization
        var normalized = EuropeEnterpriseValidator.GetNormalizedNumber(number, country);
        if (normalized != null)
        {
            return normalized;
        }

        // Fallback to original input for other regions if valid
        var result = Validate(number, country);
        return result.IsValid ? number?.Trim() : null;
    }
}
