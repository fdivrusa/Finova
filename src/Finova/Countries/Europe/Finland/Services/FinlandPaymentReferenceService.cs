using Finova.Core.Common;
using Finova.Core.PaymentReference;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.Finland.Services;

/// <summary>
/// Service for generating and validating Finnish payment references (Viitenumero).
/// </summary>
public partial class FinlandPaymentReferenceService : IsoPaymentReferenceGenerator
{
    public override string CountryCode => "FI";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public override string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalFinland) => format switch
    {
        PaymentReferenceFormat.LocalFinland => GenerateFinnishReference(rawReference),
        PaymentReferenceFormat.IsoRf => base.Generate(rawReference, format),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };


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
    public static ValidationResult ValidateStatic(string communication) => ValidateFinnishReference(communication);

    #endregion

    private static string GenerateFinnishReference(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException("Reference cannot be empty.");
        }

        if (cleanRef.Length < 3 || cleanRef.Length > 19)
        {
            throw new ArgumentException("Finnish reference data must be between 3 and 19 digits.");
        }

        var checkDigit = CalculateCheckDigit731(cleanRef);
        return $"{cleanRef}{checkDigit}";
    }

    private static ValidationResult ValidateFinnishReference(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        if (digits.Length < 4 || digits.Length > 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Finnish reference must be between 4 and 20 digits.");
        }

        var data = digits[..^1];
        var checkDigitStr = digits[^1].ToString();

        var calculatedCheckDigit = CalculateCheckDigit731(data);

        return checkDigitStr == calculatedCheckDigit.ToString()
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid check digits.");
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

