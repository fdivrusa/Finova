using Finova.Services;
using Finova.Services.Africa;
using Finova.Services.Asia;
using Finova.Services.NorthAmerica;
using Finova.Services.Oceania;
using Finova.Services.SouthAmerica;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class TaxIdValidators
{
    /// <summary>
    /// Validates that the string is a valid VAT number.
    /// Automatically detects the country from the prefix.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidVat<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(vat => EuropeVatValidator.ValidateVat(vat).IsValid)
            .WithMessage("'{PropertyName}' is not a valid VAT number.");
    }

    /// <summary>
    /// Validates that the string is a valid North American Tax ID (e.g., EIN, BN).
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "US", "CA").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidNorthAmericaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, string? countryCode = null)
    {
        return ruleBuilder
            .Must(taxId => NorthAmericaTaxIdValidator.Validate(taxId, countryCode).IsValid)
            .WithMessage("'{PropertyName}' is not a valid North American Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid North American Tax ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidNorthAmericaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, taxId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                return NorthAmericaTaxIdValidator.Validate(taxId, countryCode).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid North American Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid South American Tax ID (e.g., CNPJ, RFC).
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "BR", "MX").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidSouthAmericaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, string? countryCode = null)
    {
        return ruleBuilder
            .Must(taxId => SouthAmericaTaxIdValidator.Validate(taxId, countryCode).IsValid)
            .WithMessage("'{PropertyName}' is not a valid South American Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid South American Tax ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidSouthAmericaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, taxId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                return SouthAmericaTaxIdValidator.Validate(taxId, countryCode).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid South American Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid Asian Tax ID (e.g., USCC, GSTIN).
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "CN", "IN").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidAsiaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, string? countryCode = null)
    {
        return ruleBuilder
            .Must(taxId => AsiaTaxIdValidator.Validate(taxId, countryCode).IsValid)
            .WithMessage("'{PropertyName}' is not a valid Asian Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid Asian Tax ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidAsiaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, taxId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                return AsiaTaxIdValidator.Validate(taxId, countryCode).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid Asian Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid Oceanian Tax ID (e.g., ABN).
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "AU").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidOceaniaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, string? countryCode = null)
    {
        return ruleBuilder
            .Must(taxId => OceaniaTaxIdValidator.Validate(taxId, countryCode).IsValid)
            .WithMessage("'{PropertyName}' is not a valid Oceanian Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid Oceanian Tax ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidOceaniaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, taxId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                return OceaniaTaxIdValidator.Validate(taxId, countryCode).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid Oceanian Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid African Tax ID.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidAfricaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, string? countryCode = null)
    {
        return ruleBuilder
            .Must(taxId => AfricaTaxIdValidator.Validate(taxId, countryCode).IsValid)
            .WithMessage("'{PropertyName}' is not a valid African Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid African Tax ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidAfricaTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, taxId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                return AfricaTaxIdValidator.Validate(taxId, countryCode).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid African Tax ID.");
    }

    /// <summary>
    /// Validates that the string is a valid European Tax ID (e.g., INN for Russia).
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code.</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidEuropeTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, string? countryCode = null)
    {
        return ruleBuilder
            .Must(taxId => EuropeEnterpriseValidator.ValidateEnterpriseNumber(taxId, countryCode ?? "").IsValid)
            .WithMessage("'{PropertyName}' is not a valid European Tax/Enterprise ID.");
    }

    /// <summary>
    /// Validates that the string is a valid European Tax ID, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidEuropeTaxId<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, taxId) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                return EuropeEnterpriseValidator.ValidateEnterpriseNumber(taxId, countryCode ?? "").IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid European Tax/Enterprise ID.");
    }
}