using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Countries.Europe.Italy.Services;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian payment references (CBILL / PagoPA / IUV).
/// </summary>
public class ItalyPaymentReferenceValidator : IValidator<PaymentReferenceDetails>
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return ItalyPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidItalyIuvReference);
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
            Format = PaymentReferenceFormat.LocalItaly,
            IsValid = true
        };
    }
}
