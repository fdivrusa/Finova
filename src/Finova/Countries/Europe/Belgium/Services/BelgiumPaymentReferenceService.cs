using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.PaymentReference;
using Finova.Core.PaymentReference.Internals;


namespace Finova.Belgium.Services;

/// <summary>
/// Service for generating and validating Belgian payment references.
/// Supports both instance methods (for DI) and static methods (for direct usage).
/// </summary>
public partial class BelgiumPaymentReferenceService : IPaymentReferenceGenerator
{
    public string CountryCode => "BE";

    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    private const int OgmTotalLength = 12;

    #region Instance Methods (for Dependency Injection)

    public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.LocalBelgian) => format switch
    {
        // Domestic (OGM/VCS) - Specific to Belgium
        PaymentReferenceFormat.LocalBelgian => GenerateOgm(rawReference),

        // ISO RF - Uses the international standard logic from Core
        PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),

        _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
    };



    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Generates a Belgian OGM/VCS structured communication (+++XXX/XXXX/XXXXX+++) from a raw reference.
    /// </summary>
    /// <param name="rawReference">The raw reference data (max 10 digits)</param>
    /// <returns>Formatted Belgian OGM/VCS reference</returns>
    public static string GenerateOgmStatic(string rawReference) => GenerateOgm(rawReference);

    /// <summary>
    /// Generates an ISO 11649 (RF) international payment reference.
    /// </summary>
    /// <param name="rawReference">The raw reference data</param>
    /// <returns>ISO 11649 formatted reference (RFxx...)</returns>
    public static string GenerateIsoReferenceStatic(string rawReference) => IsoReferenceHelper.Generate(rawReference);

    /// <summary>
    /// Validates a Belgian payment reference (supports both OGM/VCS and ISO RF formats).
    /// </summary>
    /// <param name="communication">The payment reference to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static ValidationResult ValidateStatic(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
        {
            return IsoReferenceValidator.Validate(communication);
        }

        return ValidateOgm(communication);
    }

    /// <summary>
    /// Validates a Belgian OGM/VCS structured communication specifically.
    /// </summary>
    /// <param name="communication">The OGM/VCS reference to validate</param>
    /// <returns>True if valid, false otherwise</returns>
    public static ValidationResult ValidateOgmStatic(string communication) => ValidateOgm(communication);

    #endregion

    #region Private Helper Methods

    private static string GenerateOgm(string rawReference)
    {
        var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
        if (cleanRef.Length > 10)
        {
            throw new ArgumentException("OGM reference data cannot exceed 10 digits.");
        }

        // Pad the reference to 10 digits
        var paddedRef = cleanRef.PadLeft(10, '0');

        // Calculate Modulo 97 on the 10 data digits
        var mod = Modulo97Helper.Calculate(paddedRef);

        // OGM rule: Check digit is 97 if remainder is 0, otherwise the remainder itself.
        var checkDigitValue = mod == 0 ? 97 : mod;
        var checkDigit = checkDigitValue.ToString("00");

        var fullNumber = $"{paddedRef}{checkDigit}";

        return $"+++{fullNumber[..3]}/{fullNumber.Substring(3, 4)}/{fullNumber.Substring(7, 5)}+++";
    }

    private static ValidationResult ValidateOgm(string communication)
    {
        if (string.IsNullOrWhiteSpace(communication))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Communication cannot be empty.");
        }

        var digits = DigitsOnlyRegex().Replace(communication, "");

        if (digits.Length != OgmTotalLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "OGM reference must be 12 digits.");
        }

        var data = digits[..10];
        var checkDigitStr = digits.Substring(10, 2);

        // Calculate expected check digit
        var calculatedMod = Modulo97Helper.Calculate(data);
        var expectedCheckDigit = calculatedMod == 0 ? 97 : calculatedMod;

        if (int.TryParse(checkDigitStr, out var actualCheckDigit))
        {
            return expectedCheckDigit == actualCheckDigit
                ? ValidationResult.Success()
                : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid check digits.");
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Check digits must be numeric.");
    }

    #endregion
}
