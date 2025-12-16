using Finova.Core.Common;
using Finova.Core.PaymentReference;

using Finova.Countries.Europe.Slovenia.Services;

namespace Finova.Countries.Europe.Slovenia.Validators;

/// <summary>
/// Validator for Slovenian payment references (SI12).
/// </summary>
public class SloveniaPaymentReferenceValidator : IPaymentReferenceValidator
{
    public ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        return SloveniaPaymentReferenceService.ValidateStatic(reference).IsValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSlovenianSi12Reference);
    }

    public ValidationResult Validate(string? reference, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.LocalSlovenia)
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

        var clean = reference!.Replace(" ", "");

        // SI12 format: SI12 + Reference + CheckDigits (2)
        // We need to extract the "Reference" part.
        // Remove "SI12" prefix (4 chars) and last 2 chars.

        var content = clean.Substring(4, clean.Length - 6);

        return new PaymentReferenceDetails
        {
            Reference = clean,
            Content = content,
            Format = PaymentReferenceFormat.LocalSlovenia,
            IsValid = true
        };
    }
}
