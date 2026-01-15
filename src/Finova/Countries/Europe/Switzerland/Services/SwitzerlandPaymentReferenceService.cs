using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;



namespace Finova.Countries.Europe.Switzerland.Services;

/// <summary>
/// Service for generating and validating Swiss payment references (QR-Reference / ISR).
/// Uses Modulo 10 Recursive algorithm.
/// </summary>
public partial class SwitzerlandPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "CH";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Swiss QR-Reference.
    /// The reference is always 27 digits long.
    /// </summary>
    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalSwitzerland) => format switch
    {
        PaymentReferenceFormat.LocalSwitzerland => GenerateQrReference(rawReference),
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),
        _ => throw new NotSupportedException(string.Format(ValidationMessages.UnsupportedFormat, format))
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

        // Swiss QR Reference
        var digits = DigitsOnlyRegex().Replace(reference, "");
        // Last digit is check digit
        var data = digits[..^1];

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = data,
            Format = PaymentReferenceFormat.LocalSwitzerland,
            IsValid = true
        };
    }

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Swiss QR-Reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateQrReference(rawReference);

    /// <summary>
    /// Validates a Swiss QR-Reference.
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

        return ValidateQrReference(communication);
    }

    #endregion

    private static string GenerateQrReference(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException(ValidationMessages.InputCannotBeEmpty);
        }

        if (cleanRef.Length > 26)
        {
            throw new ArgumentException(ValidationMessages.InvalidSwitzerlandQrReferenceLength);
        }

        var paddedRef = cleanRef.PadLeft(26, '0');

        var checkDigit = CalculateMod10Recursive(paddedRef);

        return $"{paddedRef}{checkDigit}";
    }

    private static Core.Common.ValidationResult ValidateQrReference(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidInput, Core.Common.ValidationMessages.InputCannotBeEmpty);
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        // Swiss QR Reference is strictly 27 digits
        if (digits.Length != 27)
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidLength, Core.Common.ValidationMessages.InvalidSwitzerlandQrReferenceLength);
        }

        var data = digits[..^1];
        var checkDigitStr = digits[^1].ToString();
        var calculatedCheckDigit = CalculateMod10Recursive(data);

        return checkDigitStr == calculatedCheckDigit.ToString()
            ? Core.Common.ValidationResult.Success()
            : Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidCheckDigit, Core.Common.ValidationMessages.InvalidCheckDigit);
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

