using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Italy.Services;

/// <summary>
/// Service for generating and validating Italian payment references (CBILL / PagoPA / IUV).
/// </summary>
public partial class ItalyPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "IT";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalItaly) => format switch
    {
        PaymentReferenceFormat.LocalItaly => GenerateIuv(rawReference),
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };

    #region Static Methods

    public static string GenerateStatic(string rawReference) => GenerateIuv(rawReference);

    public static ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
            return IsoReferenceValidator.Validate(communication);

        return ValidateIuv(communication);
    }

    #endregion

    private static string GenerateIuv(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
        if (string.IsNullOrEmpty(cleanRef)) throw new ArgumentException("Reference cannot be empty.");

        // IUV is typically 18 digits.
        // If shorter, we might need to pad or calculate check digit.
        // Assuming rawReference is the base data, we append Luhn check digit.

        int checkDigit = ChecksumHelper.CalculateLuhnCheckDigit(cleanRef);
        return cleanRef + checkDigit;
    }

    private static ValidationResult ValidateIuv(string communication)
    {
        var clean = DigitsOnlyRegex().Replace(communication, "");

        // IUV is usually 15, 17 or 18 digits
        if (clean.Length < 15 || clean.Length > 18)
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidItalyIuvLength);

        if (ChecksumHelper.ValidateLuhn(clean))
            return ValidationResult.Success();

        return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidItalyIuvCheckDigit);
    }

    public PaymentReferenceDetails Parse(string reference)
    {
        var validation = ValidateStatic(reference);
        if (!validation.IsValid)
        {
            return new PaymentReferenceDetails
            {
                Reference = reference,
                Content = string.Empty,
                Format = PaymentReferenceFormat.Unknown,
                IsValid = false
            };
        }

        if (reference.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return new PaymentReferenceDetails
            {
                Reference = reference,
                Content = IsoReferenceHelper.Parse(reference),
                Format = PaymentReferenceFormat.IsoRf,
                IsValid = true
            };
        }

        // IUV
        var clean = DigitsOnlyRegex().Replace(reference, "");
        // Remove check digit (last digit)
        var data = clean.Length > 0 ? clean.Substring(0, clean.Length - 1) : clean;

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = data,
            Format = PaymentReferenceFormat.LocalItaly,
            IsValid = true
        };
    }
}
