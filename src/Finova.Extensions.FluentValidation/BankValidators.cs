using Finova.Services;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class BankValidators
{
    /// <summary>
    /// Validates that the string is a valid Bank Routing Number for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "US", "CA").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidBankRoutingNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, string countryCode)
    {
        return ruleBuilder
            .Must(routingNumber => GlobalBankValidator.ValidateRoutingNumber(countryCode, routingNumber).IsValid)
            .WithMessage($"'{{PropertyName}}' is not a valid Bank Routing Number for {countryCode}.");
    }

    /// <summary>
    /// Validates that the string is a valid Bank Routing Number, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidBankRoutingNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, routingNumber) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                if (string.IsNullOrWhiteSpace(countryCode)) return false;
                return GlobalBankValidator.ValidateRoutingNumber(countryCode, routingNumber).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid Bank Routing Number for the specified country.");
    }

    /// <summary>
    /// Validates that the string is a valid Bank Account Number for the specified country.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "SG", "JP").</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidBankAccountNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, string countryCode)
    {
        return ruleBuilder
            .Must(accountNumber => GlobalBankValidator.ValidateBankAccount(countryCode, accountNumber).IsValid)
            .WithMessage($"'{{PropertyName}}' is not a valid Bank Account Number for {countryCode}.");
    }

    /// <summary>
    /// Validates that the string is a valid Bank Account Number, using a country code from another property.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidBankAccountNumber<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> countryCodeSelector)
    {
        return ruleBuilder
            .Must((rootObject, accountNumber) =>
            {
                var countryCode = countryCodeSelector(rootObject);
                if (string.IsNullOrWhiteSpace(countryCode)) return false;
                return GlobalBankValidator.ValidateBankAccount(countryCode, accountNumber).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid Bank Account Number for the specified country.");
    }
}
