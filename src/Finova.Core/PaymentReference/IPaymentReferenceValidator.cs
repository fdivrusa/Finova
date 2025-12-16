using Finova.Core.Common;

namespace Finova.Core.PaymentReference;

/// <summary>
/// Interface for validating payment references with a specific format.
/// </summary>
public interface IPaymentReferenceValidator
{
    /// <summary>
    /// Validates a payment reference against a specific format.
    /// </summary>
    /// <param name="reference">The payment reference to validate.</param>
    /// <param name="format">The expected format.</param>
    /// <returns>A <see cref="ValidationResult"/>.</returns>
    ValidationResult Validate(string reference, PaymentReferenceFormat format);

    /// <summary>
    /// Parses a payment reference using a specific format.
    /// </summary>
    /// <param name="reference">The payment reference to parse.</param>
    /// <param name="format">The expected format.</param>
    /// <returns>The parsed details or null if invalid.</returns>
    PaymentReferenceDetails? Parse(string reference, PaymentReferenceFormat format);
}
