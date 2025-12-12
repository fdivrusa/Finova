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
    public static IRuleBuilderOptions<T, string?> MustBeValidIban<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(iban => EuropeIbanValidator.ValidateIban(iban).IsValid)
            .WithMessage("'{PropertyName}' is not a valid IBAN.");
    }

    /// <summary>
    /// Validates that the string is a valid BIC/SWIFT code (ISO 9362).
    /// </summary>
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

                return BicValidator.ValidateConsistencyWithIban(bic, iban.Substring(0, 2)).IsValid;
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
}
