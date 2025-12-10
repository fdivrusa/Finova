using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Finland.Services;

namespace Finova.Countries.Europe.Finland.Validators;

/// <summary>
/// Validator for Finnish payment references (Viitenumero).
/// </summary>
public class FinlandPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Reference cannot be empty.");

        return FinlandPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Finnish payment reference.");
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalFinland)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Format {format} is not supported by FinlandPaymentReferenceValidator.");
        }
        return Validate(reference);
    }

    public PaymentReferenceDetails? Parse(string? reference)
    {
        if (!Validate(reference).IsValid)
        {
            return null;
        }

        var clean = reference!.Replace(" ", ""); // Simple clean

        // The last digit is the check digit.
        var content = clean[..^1];

        return new PaymentReferenceDetails
        {
            Reference = clean,
            Content = content,
            Format = PaymentReferenceFormat.LocalFinland,
            IsValid = true
        };
    }
}
