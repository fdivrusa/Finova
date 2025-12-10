using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Core.PaymentReference;

public class IsoPaymentReferenceValidator : IPaymentReferenceValidator
{
    private const string IsoPrefix = "RF";

    #region Static Methods (High-Performance)

    /// <summary>
    /// Validates an ISO 11649 (RF) Reference.
    /// </summary>
    public static ValidationResult Validate(string? reference)
    {
        if (string.IsNullOrWhiteSpace(reference))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Reference cannot be empty.");
        }

        // Normalize: Remove spaces, uppercase
        string clean = reference.Replace(" ", "").ToUpperInvariant();

        // 1. Length Check (Min 5: "RFxxC", Max 25)
        if (clean.Length < 5 || clean.Length > 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Reference length must be between 5 and 25 characters.");
        }

        // 2. Prefix Check
        if (!clean.StartsWith(IsoPrefix))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Reference must start with 'RF'.");
        }

        // 3. Character Check (Alphanumeric only)
        foreach (char c in clean)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Reference contains invalid characters.");
            }
        }

        // 4. Modulo 97 Check
        string rearranged = clean[4..] + clean[0..4];
        return IbanHelper.CalculateMod97(rearranged) == 1
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid check digits.");
    }

    /// <summary>
    /// Parses an RF reference.
    /// </summary>
    public static PaymentReferenceDetails? Parse(string? reference)
    {
        if (!Validate(reference).IsValid)
        {
            return null;
        }

        string clean = reference!.Replace(" ", "").ToUpperInvariant();

        return new PaymentReferenceDetails
        {
            Reference = clean,
            Content = clean[4..], // Everything after "RFxx"
            Format = PaymentReferenceFormat.IsoRf,
            IsValid = true
        };
    }



    /// <summary>
    /// Validates an ISO 11649 (RF) Reference against a specific format.
    /// </summary>
    public ValidationResult Validate(string communication, PaymentReferenceFormat format)
    {
        if (format != PaymentReferenceFormat.IsoRf)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, $"Format {format} is not supported by IsoPaymentReferenceValidator.");
        }
        return Validate(communication);
    }

    #endregion

    #region Interface Implementation (DI Wrapper)

    ValidationResult IValidator<PaymentReferenceDetails>.Validate(string? reference)
    {
        return Validate(reference);
    }
    PaymentReferenceDetails? IValidator<PaymentReferenceDetails>.Parse(string? reference) => Parse(reference);

    #endregion
}
