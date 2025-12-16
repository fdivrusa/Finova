using Finova.Core.Common;
using Finova.Core.PaymentReference;

using Finova.Countries.Europe.Norway.Services;

namespace Finova.Countries.Europe.Norway.Validators;

/// <summary>
/// Validator for Norwegian payment references (KID).
/// </summary>
public class NorwayPaymentReferenceValidator : IValidator<PaymentReferenceDetails>
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return NorwayPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNorwayKidReference);
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
