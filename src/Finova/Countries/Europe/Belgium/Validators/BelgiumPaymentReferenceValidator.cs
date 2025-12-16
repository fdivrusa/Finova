using Finova.Belgium.Services;
using Finova.Core.Common;
using Finova.Core.PaymentReference;

namespace Finova.Belgium.Validators;

/// <summary>
/// Validator for Belgian payment references (OGM/VCS).
/// </summary>
public class BelgiumPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return BelgiumPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidBelgiumOgmVcsReference);
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalBelgian)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.UnsupportedFormat);
        }
        return Validate(reference);
    }

    public PaymentReferenceDetails? Parse(string? reference)
    {
        if (!Validate(reference).IsValid)
        {
            return null;
        }

        // OGM format: +++123/4567/89012+++
        // Clean: 123456789012
        var clean = reference!.Replace("+", "").Replace("/", "").Replace(" ", "");

        return new PaymentReferenceDetails
        {
            Reference = reference!,
            Content = clean,
            Format = PaymentReferenceFormat.LocalBelgian,
            IsValid = true
        };
    }
}
