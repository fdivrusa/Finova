using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;



namespace Finova.Countries.Europe.Finland.Services;

/// <summary>
/// Service for generating and validating Finnish payment references (Viitenumero).
/// </summary>
public partial class FinlandPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "FI";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalFinland) => format switch
    {
        PaymentReferenceFormat.LocalFinland => GenerateFinnishReference(rawReference),
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

        // Finnish Reference
        var digits = DigitsOnlyRegex().Replace(reference, "");
        // Last digit is check digit
        var data = digits[..^1];

        return new PaymentReferenceDetails
        {
            Reference = reference,
            Content = data,
            Format = PaymentReferenceFormat.LocalFinland,
            IsValid = true
        };
    }

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Finnish payment reference (Viitenumero).
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateFinnishReference(rawReference);

    /// <summary>
    /// Validates a Finnish payment reference.
    /// </summary>
    /// <summary>
    /// Validates a Finnish payment reference.
    /// </summary>
    public static ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return IsoReferenceValidator.Validate(communication);
        }

        return ValidateFinnishReference(communication);
    }

    #endregion

    private static string GenerateFinnishReference(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException(ValidationMessages.InputCannotBeEmpty);
        }

        if (cleanRef.Length < 3 || cleanRef.Length > 19)
        {
            throw new ArgumentException(ValidationMessages.InvalidFinnishReferenceLength);
        }

        var checkDigit = CalculateCheckDigit731(cleanRef);
        return $"{cleanRef}{checkDigit}";
    }

    private static ValidationResult ValidateFinnishReference(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        if (digits.Length < 4 || digits.Length > 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidFinnishReferenceLength);
        }

        var data = digits[..^1];
        var checkDigitStr = digits[^1].ToString();

        var calculatedCheckDigit = CalculateCheckDigit731(data);

        return checkDigitStr == calculatedCheckDigit.ToString()
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidCheckDigit);
    }

    private static int CalculateCheckDigit731(string data)
    {
        int[] weights = { 7, 3, 1 };
        int sum = 0;
        int weightIndex = 0;

        // Calculate from right to left
        for (int i = data.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(data[i].ToString());
            sum += digit * weights[weightIndex];
            weightIndex = (weightIndex + 1) % 3;
        }

        int nextTen = (int)Math.Ceiling(sum / 10.0) * 10;
        int diff = nextTen - sum;

        return diff == 10 ? 0 : diff;
    }
}

