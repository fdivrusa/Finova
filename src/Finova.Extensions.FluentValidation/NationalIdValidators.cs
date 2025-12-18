using Finova.Services;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class NationalIdValidators
{
    /// <summary>
    /// Validates that the string is a valid National ID for the specified country.
    /// Supports both European and Global (Asia, Americas, Oceania) countries.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "BE", "US").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidNationalId<T>(this IRuleBuilder<T, string?> ruleBuilder, string countryCode)
    {
        return ruleBuilder
            .Must(nationalId => ValidateNationalId(countryCode, nationalId).IsValid)
            .WithMessage("'{PropertyName}' is not a valid National ID for " + countryCode + ".");
    }

    /// <summary>
    /// Validates that the string is a valid National ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidNationalId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, nationalId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                if (string.IsNullOrWhiteSpace(countryCode)) return false;
                return ValidateNationalId(countryCode, nationalId).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid National ID.");
    }

    private static Core.Common.ValidationResult ValidateNationalId(string countryCode, string? nationalId)
    {
        // Try Europe first
        var europeResult = EuropeNationalIdValidator.Validate(countryCode, nationalId);
        
        // If it's an unsupported country in Europe, try Global
        if (!europeResult.IsValid && europeResult.Errors.Any(e => e.Code == Core.Common.ValidationErrorCode.UnsupportedCountry))
        {
            return GlobalIdentityValidator.ValidateNationalId(countryCode, nationalId);
        }

        return europeResult;
    }
}
