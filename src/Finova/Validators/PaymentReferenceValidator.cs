using Finova.Belgium.Services;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Denmark.Services;
using Finova.Countries.Europe.Finland.Services;
using Finova.Countries.Europe.Italy.Services;
using Finova.Countries.Europe.Norway.Services;
using Finova.Countries.Europe.Portugal.Services;
using Finova.Countries.Europe.Slovenia.Services;
using Finova.Countries.Europe.Sweden.Services;
using Finova.Countries.Europe.Switzerland.Services;

namespace Finova.Validators;

/// <summary>
/// Composite validator for payment references.
/// Validates both ISO 11649 (RF) references and local country-specific formats.
/// </summary>
/// <example>
/// <code>
/// // Validate ISO RF
/// var result = PaymentReferenceValidator.Validate("RF18123456789012");
///
/// // Validate Belgian OGM
/// var result = PaymentReferenceValidator.Validate("+++123/4567/89012+++", PaymentReferenceFormat.LocalBelgian);
/// </code>
/// </example>
public class PaymentReferenceValidator : IPaymentReferenceValidator
{
    #region Static Methods (High-Performance)

    /// <summary>
    /// Validates a payment reference using the default ISO 11649 (RF) format.
    /// </summary>
    public static ValidationResult Validate(string? communication) => Validate(communication, PaymentReferenceFormat.IsoRf);

    /// <summary>
    /// Validates a payment reference against a specific format.
    /// </summary>
    public static ValidationResult Validate(string? communication, PaymentReferenceFormat format)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
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
            PaymentReferenceFormat.LocalDenmark => DenmarkPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalItaly => ItalyPaymentReferenceService.ValidateStatic(communication),
            PaymentReferenceFormat.LocalPortugal => PortugalPaymentReferenceService.ValidateStatic(communication),
            _ => ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Unsupported format: {format}")
        };
    }

    /// <summary>
    /// Parses the payment reference using a specific format.
    /// </summary>
    public static PaymentReferenceDetails? Parse(string? reference, PaymentReferenceFormat format)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return null;
        }

        if (format == PaymentReferenceFormat.IsoRf)
        {
            return IsoPaymentReferenceValidator.Parse(reference);
        }

        var validation = Validate(reference, format);
        return validation.IsValid ? CreateDetails(reference, format) : null;
    }

    /// <summary>
    /// Parses the payment reference to identify its format and details.
    /// </summary>
    public static PaymentReferenceDetails? Parse(string? reference)
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

        if (DenmarkPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalDenmark);
        }

        if (ItalyPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalItaly);
        }

        if (PortugalPaymentReferenceService.ValidateStatic(reference).IsValid)
        {
            return CreateDetails(reference, PaymentReferenceFormat.LocalPortugal);
        }

        return null;
    }

    private static PaymentReferenceDetails CreateDetails(string reference, PaymentReferenceFormat format) => new()
    {
        Reference = reference,
        Content = reference, // For local formats, content is often the reference itself (simplified)
        Format = format,
        IsValid = true
    };

    #endregion

    #region IPaymentReferenceValidator Implementation (DI Wrapper)

    /// <inheritdoc />
    ValidationResult IPaymentReferenceValidator.Validate(string reference, PaymentReferenceFormat format) => Validate(reference, format);

    /// <inheritdoc />
    PaymentReferenceDetails? IPaymentReferenceValidator.Parse(string reference, PaymentReferenceFormat format) => Parse(reference, format);

    #endregion
}
