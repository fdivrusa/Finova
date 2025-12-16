using Finova.Services;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class EnterpriseValidators
{
    /// <summary>
    /// Validates that the string is a valid Enterprise Number for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "BE", "FR").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidEnterpriseNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, string countryCode)
    {
        return ruleBuilder
            .Must(number => EuropeEnterpriseValidator.ValidateEnterpriseNumber(number, countryCode).IsValid)
            .WithMessage($"'{{PropertyName}}' is not a valid Enterprise Number for {countryCode}.");
    }

    /// <summary>
    /// Validates that the string is a valid Enterprise Number, using a country code from another property.
    /// </summary>
    /// <param name="countryCodeSelector">Expression to select the country code property.</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidEnterpriseNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, number) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                if (string.IsNullOrWhiteSpace(countryCode))
                {
                    return true; // Skip validation if country code is missing (let other rules handle it)
                }
                return EuropeEnterpriseValidator.ValidateEnterpriseNumber(number, countryCode).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid Enterprise Number for the specified country.");
    }

    /// <summary>
    /// Validates that the string is a valid Enterprise Number for the specified type.
    /// </summary>
    /// <param name="type">The specific enterprise number type (e.g., FranceSiret).</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidEnterpriseNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, Finova.Core.Enterprise.EnterpriseNumberType type)
    {
        return ruleBuilder
            .Must(number => EuropeEnterpriseValidator.ValidateEnterpriseNumber(number, type).IsValid)
            .WithMessage($"'{{PropertyName}}' is not a valid {type}.");
    }
}
