using Finova.Belgium.Services;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Finland.Services;
using Finova.Countries.Europe.Norway.Services;
using Finova.Countries.Europe.Slovenia.Services;
using Finova.Countries.Europe.Sweden.Services;
using Finova.Countries.Europe.Switzerland.Services;

namespace Finova.Validators;

/// <summary>
/// Composite validator for payment references.
/// Validates both ISO 11649 (RF) references and local country-specific formats.
/// </summary>
public class PaymentReferenceValidator : IPaymentReferenceValidator
{
    /// <summary>
    /// Validates a payment reference.
    /// </summary>
    /// <param name="communication">The payment reference to validate.</param>
    /// <returns>Validation result.</returns>
    /// <summary>
    /// Validates a payment reference.
    /// </summary>
    /// <param name="communication">The payment reference to validate.</param>
    /// <returns>Validation result.</returns>
    public ValidationResult Validate(string? communication) => Validate(communication, PaymentReferenceFormat.IsoRf);

    /// <summary>
    /// Validates a payment reference against a specific format.
    /// </summary>
    /// <param name="communication">The payment reference to validate.</param>
    /// <param name="format">The expected format.</param>
    /// <returns>Validation result.</returns>
    public ValidationResult Validate(string? communication, PaymentReferenceFormat format)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        return format switch
        {
            PaymentReferenceFormat.IsoRf => IsoPaymentReferenceValidator.Validate(communication),
            PaymentReferenceFormat.LocalBelgian => BelgiumPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalFinland => FinlandPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalNorway => NorwayPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalSweden => SwedenPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalSwitzerland => SwitzerlandPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalSlovenia => SloveniaPaymentReferenceService.ValidateStatic(communication),
            _ => ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Unsupported format: {format}")
        };
    }

    /// <summary>
    /// Parses the payment reference to identify its format and details.
    /// </summary>
    /// <param name="reference">The payment reference to parse.</param>
    /// <returns>The parsed details, or null if invalid.</returns>
    public PaymentReferenceDetails? Parse(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return null;
        }

        // ISO RF
        if (reference.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return IsoPaymentReferenceValidator.Parse(reference);
        }

        // For local formats, we return a generic details object if valid
        // Note: Ideally, we would have Parse methods in each service to return specific details,
        // but for now we'll return a generic valid result with the detected format.

        if (BelgiumPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalBelgian);
        }

        if (FinlandPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalFinland);
        }

        if (NorwayPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalNorway);
        }

        if (SwedenPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalSweden);
        }

        if (SwitzerlandPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalSwitzerland);
        }

        if (SloveniaPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalSlovenia);
        }

        return null;
    }

    private static PaymentReferenceDetails CreateDetails(string reference, PaymentReferenceFormat format) => new PaymentReferenceDetails
    {
        Reference = reference,
        Content = reference, // For local formats, content is often the reference itself (simplified)
        Format = format,
        IsValid = true
    };
}
