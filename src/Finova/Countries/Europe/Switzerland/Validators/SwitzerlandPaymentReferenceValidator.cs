using Finova.Core.Common;
using Finova.Core.PaymentReference;

using Finova.Countries.Europe.Switzerland.Services;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validator for Swiss payment references (QR-Reference).
/// </summary>
public class SwitzerlandPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Reference cannot be empty.");
        }

        return SwitzerlandPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Swiss QR-Reference.");
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalSwitzerland)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Format {format} is not supported by SwitzerlandPaymentReferenceValidator.");
        }
        return Validate(reference);
    }

    public PaymentReferenceDetails? Parse(string? reference)
    {
        if (!Validate(reference).IsValid)
        {
            return null;
        }

        var clean = reference!.Replace(" ", "");

        // QR-Reference: Data (26) + CheckDigit (1) = 27 digits.
        // Content is the first 26 digits.
        var content = clean[..^1];

        return new PaymentReferenceDetails
        {
            Reference = clean,
            Content = content,
            Format = PaymentReferenceFormat.LocalSwitzerland,
            IsValid = true
        };
    }
}
