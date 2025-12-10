using Finova.Core.Common;

namespace Finova.Core.PaymentCard;

public interface IPaymentCardValidator : IValidator<PaymentCardDetails>
{
    /// <summary>
    /// Validates the card number using the Luhn algorithm.
    /// </summary>
    /// <param name="cardNumber">The card number to validate.</param>
    /// <returns>A validation result indicating whether the card number is valid.</returns>
    ValidationResult ValidateLuhn(string? cardNumber);

    /// <summary>
    /// Identifies the payment card brand based on the specified card number.
    /// </summary>
    /// <param name="cardNumber">The card number to analyze. Can be null or empty; in such cases, the method returns PaymentCardBrand.Unknown.</param>
    /// <returns>A value of the PaymentCardBrand enumeration that represents the detected card brand. Returns
    /// PaymentCardBrand.Unknown if the brand cannot be determined.</returns>
    PaymentCardBrand GetBrand(string? cardNumber);

    /// <summary>
    /// Validates the CVV based on the card brand.
    /// </summary>
    /// <param name="cvv">The CVV to validate.</param>
    /// <param name="brand">The brand of the payment card.</param>
    /// <returns>A validation result indicating whether the CVV is valid.</returns>
    ValidationResult ValidateCvv(string? cvv, PaymentCardBrand brand);

    /// <summary>
    /// Validates whether the specified expiration month and year represent a valid, non-expired date.
    /// </summary>
    /// <param name="month">The expiration month as an integer from 1 (January) to 12 (December).</param>
    /// <param name="year">The expiration year as a four-digit integer (e.g., 2025).</param>
    /// <returns>A ValidationResult indicating whether the expiration date is valid and not in the past.</returns>
    ValidationResult ValidateExpiration(int month, int year);
}
