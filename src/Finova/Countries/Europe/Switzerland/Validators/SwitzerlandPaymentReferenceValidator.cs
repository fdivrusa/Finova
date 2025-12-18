using Finova.Core.Common;
using Finova.Core.PaymentReference;

using Finova.Countries.Europe.Switzerland.Services;

namespace Finova.Countries.Europe.Switzerland.Validators;

/// <summary>
/// Validator for Swiss payment references (QR-Reference).
/// </summary>
public class SwitzerlandPaymentReferenceValidator : IValidator<PaymentReferenceDetails>
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return SwitzerlandPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSwissQrReference);
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
