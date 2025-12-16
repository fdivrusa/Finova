using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Services;
using FluentValidation;
using Finova.Validators;
using Finova.Core.PaymentReference;

namespace Finova.Extensions.FluentValidation;

public static class FinovaValidators
{
    /// <summary>
    /// Validates that the string is a valid IBAN.
    /// Automatically detects the country and applies the correct rules (e.g. Italy vs Germany).
    /// </summary>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Iban).MustBeValidIban();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> MustBeValidIban<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(iban => EuropeIbanValidator.ValidateIban(iban).IsValid)
            .WithMessage("'{PropertyName}' is not a valid IBAN.");
    }

    /// <summary>
    /// Validates that the string is a valid BIC/SWIFT code (ISO 9362).
    /// </summary>
    /// <example>
    /// <code>
    /// RuleFor(x => x.Bic).MustBeValidBic();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> MustBeValidBic<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(x => BicValidator.Validate(x).IsValid)
            .WithMessage("'{PropertyName}' is not a valid BIC/SWIFT code.");
    }

    /// <summary>
    /// Validates that the string is a valid credit card number (Luhn Check).
    /// Note: This only checks mathematical validity, not existence.
    /// </summary>
    /// <example>
    /// <code>
    /// RuleFor(x => x.CardNumber).MustBeValidPaymentCard();
    /// </code>
    /// </example>
    public static IRuleBuilderOptions<T, string?> MustBeValidPaymentCard<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(x => PaymentCardValidator.Validate(x).IsValid)
            .WithMessage("'{PropertyName}' is not a valid card number.");
    }

    /// <summary>
    /// Validates that the BIC country code matches the IBAN country code.
    /// </summary>
    /// <param name="ibanSelector">Expression to select the IBAN property to compare against.</param>
    public static IRuleBuilderOptions<T, string?> MustMatchIbanCountry<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> ibanSelector)
    {
        return ruleBuilder
            .Must((rootObject, bic) =>
            {
                var iban = ibanSelector(rootObject);
                if (string.IsNullOrWhiteSpace(iban))
                {
                    return true;
                }

                return BicValidator.ValidateConsistencyWithIban(bic, iban[..2]).IsValid;
            })
            .WithMessage("The BIC country does not match the IBAN country.");
    }

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
    /// Validates that the string is a valid Payment Reference.
    /// Supports ISO 11649 (RF) and local formats (BE, FI, NO, SE, CH, SI).
    /// </summary>
    /// <param name="format">The expected format (default is IsoRf).</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidPaymentReference<T>(this IRuleBuilder<T, string?> ruleBuilder, PaymentReferenceFormat format = PaymentReferenceFormat.IsoRf)
    {
        return ruleBuilder
            .Must(reference => PaymentReferenceValidator.Validate(reference, format).IsValid)
            .WithMessage("'{PropertyName}' is not a valid payment reference.");
    }
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
