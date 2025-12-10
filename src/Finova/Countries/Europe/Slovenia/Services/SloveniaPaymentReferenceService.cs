using Finova.Core.PaymentReference;
using System.Text.RegularExpressions;

using Finova.Core.PaymentReference.Internals;
using Finova.Core.Common;


namespace Finova.Countries.Europe.Slovenia.Services;

/// <summary>
/// Service for generating and validating Slovenian payment references (SI12).
/// Uses Modulo 97 algorithm.
/// </summary>
public partial class SloveniaPaymentReferenceService : IsoPaymentReferenceGenerator
{
    public override string CountryCode => "SI";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    /// <summary>
    /// Generates a Slovenian SI12 reference.
    /// Format: SI12 + Reference + CheckDigits (Mod 97).
    /// </summary>
    public override string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalSlovenia) => format switch
    {
        PaymentReferenceFormat.LocalSlovenia => GenerateSi12(rawReference),
        PaymentReferenceFormat.IsoRf => base.Generate(rawReference, format),
        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };



    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Slovenian SI12 reference.
    /// </summary>
    public static string GenerateStatic(string rawReference) => GenerateSi12(rawReference);

    /// <summary>
    /// Validates a Slovenian SI12 reference.
    /// </summary>
    /// <summary>
    /// Validates a Slovenian SI12 reference.
    /// </summary>
    public static Core.Common.ValidationResult ValidateStatic(string communication) => ValidateSi12(communication);

    #endregion

    private static string GenerateSi12(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");

        if (string.IsNullOrEmpty(cleanRef))
        {
            throw new ArgumentException("Reference cannot be empty.");
        }

        // Calculate Mod 97 on the reference
        // We assume the check digits are appended to the reference.
        // Similar to Belgian OGM but without the +++ formatting and fixed length?
        // Or maybe we should pad it?
        // Let's assume standard Mod 97 calculation on the number.

        var mod = Modulo97Helper.Calculate(cleanRef);
        var checkDigitValue = mod == 0 ? 97 : mod;
        var checkDigits = checkDigitValue.ToString("00");

        return $"SI12{cleanRef}{checkDigits}";
    }

    private static ValidationResult ValidateSi12(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        // Must start with SI12
        if (!communication.StartsWith("SI12"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Slovenian reference must start with SI12.");
        }

        var digits = DigitsOnlyRegex().Replace(communication.Substring(4), ""); // Remove SI12 prefix

        if (digits.Length < 3) // At least 1 digit data + 2 check digits
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Slovenian reference is too short.");
        }

        var data = digits[..^2];
        var checkDigitStr = digits.Substring(digits.Length - 2);

        var mod = Modulo97Helper.Calculate(data);
        var expectedCheckDigitValue = mod == 0 ? 97 : mod;

        return checkDigitStr == expectedCheckDigitValue.ToString("00")
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid check digits.");
    }
}

