using Finova.Core.PaymentReference;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.Norway.Services;

/// <summary>
/// Service for generating and validating Norwegian payment references (KID - Kundeidentifikasjonsnummer).
/// Supports both Modulo 10 (Luhn) and Modulo 11 algorithms.
/// </summary>
public partial class NorwayPaymentReferenceService : IsoPaymentReferenceGenerator
{
    public override string CountryCode => "NO";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Norwegian KID reference.
    /// By default, uses Modulo 10 (Luhn) which is the most common.
    /// To use Modulo 11, append "-11" to the raw reference (e.g., "12345-11").
    /// </summary>
    public override string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalNorway) => format switch
    {
        PaymentReferenceFormat.LocalNorway => GenerateKid(rawReference),
        PaymentReferenceFormat.IsoRf => base.Generate(rawReference, format),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };



    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Norwegian KID reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateKid(rawReference);

    /// <summary>
    /// Validates a Norwegian KID reference.
    /// </summary>
    /// <summary>
    /// Validates a Norwegian KID reference.
    /// </summary>
    public static Core.Common.ValidationResult ValidateStatic(string communication) => ValidateKid(communication);

    #endregion

    private static string GenerateKid(string rawReference)
    {
        // Check if Mod11 is requested via suffix
        bool useMod11 = rawReference.EndsWith("-11");
        string refToClean = useMod11 ? rawReference[..^3] : rawReference;
        var cleanRef = DigitsOnlyRegex().Replace(refToClean, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException("Reference cannot be empty.");
        }

        if (cleanRef.Length < 2 || cleanRef.Length > 24)
        {
            throw new ArgumentException("Norwegian KID reference data must be between 2 and 24 digits.");
        }

        if (useMod11)
        {
            var checkDigit = CalculateMod11(cleanRef);
            if (checkDigit == "-")
            {
                throw new ArgumentException("Invalid reference data for Modulo 11 (result is 10).");
            }
            return $"{cleanRef}{checkDigit}";
        }
        else
        {
            var checkDigit = CalculateLuhn(cleanRef);
            return $"{cleanRef}{checkDigit}";
        }
    }

    private static Core.Common.ValidationResult ValidateKid(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        if (digits.Length < 3 || digits.Length > 25)
        {
            return Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidLength, "Norwegian KID reference must be between 3 and 25 digits.");
        }

        var data = digits[..^1];
        var checkDigitStr = digits[^1].ToString();

        // Try Mod 10 (Luhn) first as it's most common
        var luhnDigit = CalculateLuhn(data);
        if (checkDigitStr == luhnDigit.ToString())
        {
            return Core.Common.ValidationResult.Success();
        }

        // Try Mod 11
        var mod11Digit = CalculateMod11(data);
        return (mod11Digit != "-" && checkDigitStr == mod11Digit)
            ? Core.Common.ValidationResult.Success()
            : Core.Common.ValidationResult.Failure(Core.Common.ValidationErrorCode.InvalidCheckDigit, "Invalid check digits (neither Mod10 nor Mod11 matched).");
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

    private static string CalculateMod11(string data)
    {
        int sum = 0;
        int weight = 2;

        for (int i = data.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(data[i].ToString());
            sum += digit * weight;
            weight++;
            if (weight > 7) weight = 2;
        }

        int remainder = sum % 11;
        if (remainder == 0)
        {
            return "0";
        }
        if (remainder == 1)
        {
            return "-"; // 11 - 1 = 10, which is invalid for single digit
        }
        return (11 - remainder).ToString();
    }
}

