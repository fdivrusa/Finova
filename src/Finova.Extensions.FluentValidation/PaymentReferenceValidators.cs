using Finova.Core.PaymentReference;
using Finova.Validators;
using FluentValidation;

namespace Finova.Extensions.FluentValidation;

public static class PaymentReferenceValidators
{
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
