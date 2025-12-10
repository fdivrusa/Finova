using Finova.Core.Common;
using Finova.Core.PaymentReference;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.Sweden.Services;

/// <summary>
/// Service for generating and validating Swedish payment references (OCR / Bankgiro).
/// Uses Modulo 10 (Luhn) with an optional length check digit.
/// </summary>
public partial class SwedenPaymentReferenceService : IsoPaymentReferenceGenerator
{
    public override string CountryCode => "SE";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Swedish OCR reference.
    /// By default, appends a length check digit and then a Luhn check digit.
    /// Format: [Data][LengthDigit][CheckDigit]
    /// </summary>
    public override string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalSweden) => format switch
    {
        PaymentReferenceFormat.LocalSweden => GenerateOcr(rawReference),
        PaymentReferenceFormat.IsoRf => base.Generate(rawReference, format),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };



    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Swedish OCR reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateOcr(rawReference);

    /// <summary>
    /// Validates a Swedish OCR reference.
    /// </summary>
    /// <summary>
    /// Validates a Swedish OCR reference.
    /// </summary>
    public static Core.Common.ValidationResult ValidateStatic(string communication) => ValidateOcr(communication);

    #endregion

    private static string GenerateOcr(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException("Reference cannot be empty.");
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        if (digits.Length < 2 || digits.Length > 25)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Swedish OCR reference must be between 2 and 25 digits.");
        }

        // 1. Validate Check Digit (Last digit, Mod 10 of everything before it)
        var dataWithLength = digits[..^1];
        var checkDigitStr = digits[^1].ToString();
        var calculatedCheckDigit = CalculateLuhn(dataWithLength);

        if (checkDigitStr != calculatedCheckDigit.ToString())
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid check digits.");
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
            : ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid length digit.");
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
                if (digit > 9) digit -= 9;
            }
            sum += digit;
            doubleDigit = !doubleDigit;
        }

        return (10 - (sum % 10)) % 10;
    }
}

