using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;



namespace Finova.Countries.Europe.Sweden.Services;

/// <summary>
/// Service for generating and validating Swedish payment references (OCR / Bankgiro).
/// Uses Modulo 10 (Luhn) with an optional length check digit.
/// </summary>
public partial class SwedenPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "SE";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Swedish OCR reference.
    /// By default, appends a length check digit and then a Luhn check digit.
    /// Format: [Data][LengthDigit][CheckDigit]
    /// </summary>
    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalSweden) => format switch
    {
        PaymentReferenceFormat.LocalSweden => GenerateOcr(rawReference),
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };

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

        // Swedish OCR
        var digits = DigitsOnlyRegex().Replace(reference, "");
        // Last digit is check digit, second last is length digit
        var data = digits[..^2];

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = data,
            Format = PaymentReferenceFormat.LocalSweden,
            IsValid = true
        };
    }

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Swedish OCR reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateOcr(rawReference);

    /// <summary>
    /// Validates a Swedish OCR reference.
    /// </summary>
    public static Core.Common.ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidInput, Core.Common.ValidationMessages.InputCannotBeEmpty);
        }

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return IsoReferenceValidator.Validate(communication);
        }

        return ValidateOcr(communication);
    }

    #endregion

    private static string GenerateOcr(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException(ValidationMessages.InputCannotBeEmpty);
        }

        if (cleanRef.Length > 23)
        {
            throw new ArgumentException("Swedish OCR reference data cannot exceed 23 digits (to allow for 2 check digits).");
        }

        // Calculate length digit
        // Length = Data + LengthDigit + CheckDigit
        // So Length = cleanRef.Length + 2
        int totalLength = cleanRef.Length + 2;
        int lengthDigit = totalLength % 10;

        var refWithLength = $"{cleanRef}{lengthDigit}";
        var checkDigit = CalculateLuhn(refWithLength);

        return $"{refWithLength}{checkDigit}";
    }

    private static Core.Common.ValidationResult ValidateOcr(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        if (digits.Length < 2 || digits.Length > 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSwedishOcrLength);
        }

        // 1. Validate Check Digit (Last digit, Mod 10 of everything before it)
        var dataWithLength = digits[..^1];
        var checkDigitStr = digits[^1].ToString();
        var calculatedCheckDigit = CalculateLuhn(dataWithLength);

        if (checkDigitStr != calculatedCheckDigit.ToString())
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);
        }

        // 2. Validate Length Digit (Second to last digit)
        // This is optional in some implementations ("Soft" vs "Hard" level), 
        // but standard Bankgiro usually involves it.
        // We will validate it if it matches the length.

        var lengthDigitStr = digits[^2].ToString();
        int actualLength = digits.Length;
        int expectedLengthDigit = actualLength % 10;

        // Note: Some older systems might not use the length digit. 
        // However, for generation we enforce it. For validation, we can be stricter.
        // Let's enforce it for now as it's the standard "Hard" level.
        return lengthDigitStr == expectedLengthDigit.ToString()
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLengthDigit);
    }

    private static int CalculateLuhn(string data)
    {
        int sum = 0;
        bool doubleDigit = true;

        for (int i = data.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(data[i].ToString());
            if (doubleDigit)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }
            sum += digit;
            doubleDigit = !doubleDigit;
        }

        return (10 - (sum % 10)) % 10;
    }
}

