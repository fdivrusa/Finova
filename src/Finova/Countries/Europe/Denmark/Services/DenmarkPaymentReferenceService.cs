using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Denmark.Services;

/// <summary>
/// Service for generating and validating Danish payment references (FIK/GIK).
/// </summary>
public partial class DenmarkPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "DK";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalDenmark) => format switch
    {
        PaymentReferenceFormat.LocalDenmark => GenerateFik(rawReference),
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };

    #region Static Methods

    public static string GenerateStatic(string rawReference) => GenerateFik(rawReference);

    public static ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
            return IsoReferenceValidator.Validate(communication);

        return ValidateFik(communication);
    }

    #endregion

    private static string GenerateFik(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
        if (string.IsNullOrEmpty(cleanRef)) throw new ArgumentException("Reference cannot be empty.");

        // FIK usually uses Mod10 (Luhn). We append the check digit.
        // Common length is 15 digits (Type 71).

        int checkDigit = ChecksumHelper.CalculateLuhnCheckDigit(cleanRef);
        return $"+71<{cleanRef}{checkDigit}"; // Using Type 71 format as default
    }

    private static ValidationResult ValidateFik(string communication)
    {
        // Handle FIK format like +71<123456789012345
        string refPart = communication;
        if (communication.Contains('<'))
        {
            var parts = communication.Split('<');
            if (parts.Length > 1)
            {
                refPart = parts[1];
            }
        }

        var clean = DigitsOnlyRegex().Replace(refPart, "");

        // FIK references are usually between 10 and 16 digits
        if (clean.Length < 10 || clean.Length > 16)
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidDenmarkFikLength);

        if (ChecksumHelper.ValidateLuhn(clean))
            return ValidationResult.Success();

        return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidDenmarkFikCheckDigit);
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

        // FIK
        string content = reference;
        if (reference.Contains('<'))
        {
            var parts = reference.Split('<');
            if (parts.Length > 1)
            {
                content = parts[1];
            }
        }

        var clean = DigitsOnlyRegex().Replace(content, "");
        // Remove check digit
        var data = clean.Length > 0 ? clean.Substring(0, clean.Length - 1) : clean;

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = data,
            Format = PaymentReferenceFormat.LocalDenmark,
            IsValid = true
        };
    }
}
