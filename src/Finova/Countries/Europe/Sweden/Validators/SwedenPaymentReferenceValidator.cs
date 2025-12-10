using Finova.Core.Common;
using Finova.Core.PaymentReference;

using Finova.Countries.Europe.Sweden.Services;

namespace Finova.Countries.Europe.Sweden.Validators;

/// <summary>
/// Validator for Swedish payment references (OCR).
/// </summary>
public class SwedenPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Reference cannot be empty.");
        }

        return SwedenPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Swedish OCR reference.");
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalSweden)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Format {format} is not supported by SwedenPaymentReferenceValidator.");
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

        // OCR: Data + LengthDigit (1) + CheckDigit (1)
        // So remove last 2 digits.
        var content = clean[..^2];

        return new PaymentReferenceDetails
        {
            Reference = clean,
            Content = content,
            Format = PaymentReferenceFormat.LocalSweden,
            IsValid = true
        };
    }
}
