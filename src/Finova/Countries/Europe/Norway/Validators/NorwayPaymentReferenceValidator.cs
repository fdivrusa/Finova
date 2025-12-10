using Finova.Core.Common;
using Finova.Core.PaymentReference;

using Finova.Countries.Europe.Norway.Services;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norwegian payment references (KID).
/// </summary>
public class NorwayPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Reference cannot be empty.");
        }

        return NorwayPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Norwegian KID reference.");
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalNorway)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Format {format} is not supported by NorwayPaymentReferenceValidator.");
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

        // KID: Data + CheckDigit (1 digit)
        var content = clean[..^1];

        return new PaymentReferenceDetails
        {
            Reference = clean,
            Content = content,
            Format = PaymentReferenceFormat.LocalNorway,
            IsValid = true
        };
    }
}
