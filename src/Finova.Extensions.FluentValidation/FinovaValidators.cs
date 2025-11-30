using Finova.Core.Validators;
using Finova.Services;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class FinovaValidators
{
    /// <summary>
    /// Validates that the string is a valid IBAN.
    /// Automatically detects the country and applies the correct rules (e.g. Italy vs Germany).
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidIban<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(EuropeIbanValidator.Validate)
            .WithMessage("'{PropertyName}' is not a valid IBAN.");
    }

    /// <summary>
    /// Validates that the string is a valid BIC/SWIFT code (ISO 9362).
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidBic<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(BicValidator.Validate)
            .WithMessage("'{PropertyName}' is not a valid BIC/SWIFT code.");
    }

    /// <summary>
    /// Validates that the string is a valid credit card number (Luhn Check).
    /// Note: This only checks mathematical validity, not existence.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> MustBeValidPaymentCard<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(PaymentCardValidator.Validate)
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

                return BicValidator.IsConsistentWithIban(bic, iban.Substring(0, 2));
            })
            .WithMessage("The BIC country does not match the IBAN country.");
    }
}
