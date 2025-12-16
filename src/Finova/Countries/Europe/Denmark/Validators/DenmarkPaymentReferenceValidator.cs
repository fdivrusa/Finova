using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Denmark.Services;

namespace Finova.Countries.Europe.Denmark.Validators;

/// <summary>
/// Validator for Danish payment references (FIK/GIK).
/// </summary>
public class DenmarkPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return DenmarkPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidDenmarkPaymentReference);
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalDenmark)
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

        // FIK format: +71<123456789012345+12345678
        // Clean: Digits only
        // Note: DenmarkPaymentReferenceService.ValidateStatic handles cleaning.
        // We should probably expose a Parse method in the Service or replicate logic.
        // For now, just return the cleaned reference.

        // Simple clean for details
        var clean = System.Text.RegularExpressions.Regex.Replace(reference!, @"[^\d]", "");

        return new PaymentReferenceDetails
        {
            Reference = reference!,
            Content = clean,
            Format = PaymentReferenceFormat.LocalDenmark,
            IsValid = true
        };
    }
}
