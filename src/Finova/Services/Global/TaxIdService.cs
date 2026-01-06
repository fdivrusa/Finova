using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Services.Africa;
using Finova.Services.Asia;
using Finova.Services.NorthAmerica;
using Finova.Services.SouthAmerica;

namespace Finova.Services;

/// <summary>
/// Default implementation of <see cref="ITaxIdService"/>.
/// Validates Tax IDs by delegating to registered <see cref="ITaxIdValidator"/> implementations.
/// </summary>
public class TaxIdService : ITaxIdService
{
    private readonly IEnumerable<ITaxIdValidator> _validators;

    public TaxIdService(IEnumerable<ITaxIdValidator> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc/>
    public ValidationResult Validate(string countryCode, string? taxId)
    {
        if (string.IsNullOrWhiteSpace(countryCode))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Try regional static validators first if no DI registered ones match or as primary
        var country = countryCode.ToUpperInvariant();

        // This is a hybrid approach: prefer DI validators, fallback to static region routing
        var registeredValidators = _validators.Where(v => v.CountryCode.Equals(country, StringComparison.OrdinalIgnoreCase)).ToList();

        if (registeredValidators.Count > 0)
        {
            List<ValidationError> errors = [];
            foreach (var validator in registeredValidators)
            {
                var res = validator.Validate(taxId);
                if (res.IsValid)
                {
                    return res;
                }

                errors.AddRange(res.Errors);
            }
            return ValidationResult.Failure(errors);
        }

        // Fallback to static regional routers
        return country switch
        {
            "DZ" or "AO" or "EG" or "MA" or "NG" or "SN" or "CI" or "TN" or "ZA"
                => AfricaTaxIdValidator.Validate(taxId, country),
            "AR" or "BR" or "CL" or "CO" or "MX"
                => SouthAmericaTaxIdValidator.Validate(taxId, country),
            "US" or "CA" or "CR" or "DO" or "SV" or "GT" or "HN" or "NI"
                => NorthAmericaTaxIdValidator.Validate(taxId, country),
            "CN" or "IN" or "JP" or "KR" or "SG" or "KZ" or "VN"
                => AsiaTaxIdValidator.Validate(taxId, country),
            _ => ValidationResult.Failure(ValidationErrorCode.UnsupportedCountry, string.Format(ValidationMessages.NoTaxIdValidatorRegistered, countryCode))
        };
    }
}