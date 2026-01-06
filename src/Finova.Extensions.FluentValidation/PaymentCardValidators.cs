using Finova.Core.PaymentCard;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class PaymentCardValidators
{
    /// <summary>
    /// Validates that the string is a valid payment card number (Luhn check).
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
    /// Validates that the string is a valid payment card number for a specific brand (e.g. Visa, Mastercard).
    /// </summary>
    /// <param name="brand">The expected card brand.</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidPaymentCardForBrand<T>(this IRuleBuilder<T, string?> ruleBuilder, PaymentCardBrand brand)
    {
        return ruleBuilder
            .Must(x =>
            {
                if (!PaymentCardValidator.Validate(x).IsValid)
                {
                    return false;
                }

                return PaymentCardValidator.GetBrand(x) == brand;
            })
            .WithMessage($"'{{PropertyName}}' is not a valid {brand} card number.");
    }

    /// <summary>
    /// Validates that the CVV is valid for the given card number.
    /// </summary>
    /// <param name="cardNumberSelector">Expression to select the card number property.</param>
    public static IRuleBuilderOptions<T, string?> MustBeValidCvv<T>(this IRuleBuilder<T, string?> ruleBuilder, Func<T, string?> cardNumberSelector)
    {
        return ruleBuilder
            .Must((rootObject, cvv) =>
            {
                var cardNumber = cardNumberSelector(rootObject);
                if (string.IsNullOrWhiteSpace(cardNumber))
                {
                    return true; // Skip if card number is missing
                }

                var brand = PaymentCardValidator.GetBrand(cardNumber);
                return PaymentCardValidator.ValidateCvv(cvv, brand).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid CVV for this card.");
    }

    /// <summary>
    /// Validates that the card expiration date is valid and in the future.
    /// </summary>
    /// <param name="yearSelector">Expression to select the expiration year property.</param>
    public static IRuleBuilderOptions<T, int> MustBeValidExpirationDate<T>(this IRuleBuilder<T, int> ruleBuilder, Func<T, int> yearSelector)
    {
        return ruleBuilder
            .Must((rootObject, month) =>
            {
                var year = yearSelector(rootObject);
                return PaymentCardValidator.ValidateExpiration(month, year).IsValid;
            })
            .WithMessage("'{PropertyName}' is not a valid expiration date.");
    }
}
