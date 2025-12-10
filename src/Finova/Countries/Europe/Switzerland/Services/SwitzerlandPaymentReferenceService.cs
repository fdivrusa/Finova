using Finova.Core.PaymentReference;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.Switzerland.Services;

/// <summary>
/// Service for generating and validating Swiss payment references (QR-Reference / ISR).
/// Uses Modulo 10 Recursive algorithm.
/// </summary>
public partial class SwitzerlandPaymentReferenceService : IsoPaymentReferenceGenerator
{
    public override string CountryCode => "CH";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Swiss QR-Reference.
    /// The reference is always 27 digits long.
    /// </summary>
    public override string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalSwitzerland) => format switch
    {
        PaymentReferenceFormat.LocalSwitzerland => GenerateQrReference(rawReference),
        PaymentReferenceFormat.IsoRf => base.Generate(rawReference, format),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };



    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Swiss QR-Reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateQrReference(rawReference);

    /// <summary>
    /// Validates a Swiss QR-Reference.
    /// </summary>
    public static Core.Common.ValidationResult ValidateStatic(string communication) => ValidateQrReference(communication);

    #endregion

    private static string GenerateQrReference(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException("Reference cannot be empty.");
        }

        if (cleanRef.Length > 26)
        {
            throw new ArgumentException("Swiss QR reference data cannot exceed 26 digits.");
        }

        var paddedRef = cleanRef.PadLeft(26, '0');

        var checkDigit = CalculateMod10Recursive(paddedRef);

        return $"{paddedRef}{checkDigit}";
    }

    private static Core.Common.ValidationResult ValidateQrReference(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        // Swiss QR Reference is strictly 27 digits
        if (digits.Length != 27)
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidLength, "Swiss QR reference must be 27 digits.");
        }

        var data = digits[..^1];
        var checkDigitStr = digits[^1].ToString();
        var calculatedCheckDigit = CalculateMod10Recursive(data);

        return checkDigitStr == calculatedCheckDigit.ToString()
            ? Core.Common.ValidationResult.Success()
            : Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidCheckDigit, "Invalid check digits.");
    }

    private static int CalculateMod10Recursive(string data)
    {
        // Table for Modulo 10 Recursive
        int[] table = { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };
        int carry = 0;

        foreach (char c in data)
        {
            int digit = int.Parse(c.ToString());
            int index = (carry + digit) % 10;
            carry = table[index];
        }

        return (10 - carry) % 10;
    }
}

