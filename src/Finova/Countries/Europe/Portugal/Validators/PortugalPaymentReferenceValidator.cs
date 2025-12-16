using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Portugal.Services;

namespace Finova.Countries.Europe.Portugal.Validators;

/// <summary>
/// Validator for Portuguese payment references (Multibanco).
/// </summary>
public class PortugalPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return PortugalPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalPortugal)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.UnsupportedFormat, format));
        }
        return Validate(reference);
    }

    public PaymentReferenceDetails? Parse(string? reference)
    {
        if (!Validate(reference).IsValid)
        {
            return null;
        }

        var clean = System.Text.RegularExpressions.Regex.Replace(reference!, @"[^\d]", "");

        return new PaymentReferenceDetails
        {
            Reference = reference!,
            Content = clean,
            Format = PaymentReferenceFormat.LocalPortugal,
            IsValid = true
        };
    }
}
