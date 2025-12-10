using Finova.Core.Common;

namespace Finova.Core.PaymentReference;

public interface IPaymentReferenceValidator : IValidator<PaymentReferenceDetails>
{
    /// <summary>
    /// Validates a payment reference against a specific format.
    /// </summary>
    ValidationResult Validate(string communication, PaymentReferenceFormat format);
}
