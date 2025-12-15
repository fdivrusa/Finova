using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;

namespace Finova.Countries.Europe.Greenland.Validators;

/// <summary>
/// Validator for Greenland CVR (Centrale Virksomhedsregister).
/// Format: 8 digits.
/// </summary>
public partial class GreenlandCvrValidator : IEnterpriseValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "GL";

    public ValidationResult Validate(string? instance) => ValidateCvr(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Greenland CVR number.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateCvr(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Enterprise number cannot be empty.");
        }

        // Remove "GL" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("GL"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "CVR number must be 8 digits.");
        }

        // Weights: [2, 7, 6, 5, 4, 3, 2]
        int[] weights = { 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 7; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        // The check digit is the 8th digit
        int checkDigit = digits[7] - '0';
        sum += checkDigit * 1; // Implicit weight 1 for check digit? No, spec says sum % 11 == 0.
                               // Let's re-read the spec provided: "Weights: [2, 7, 6, 5, 4, 3, 2] applied to all 8 digits."
                               // Wait, 7 weights for 8 digits?
                               // "Weights: [2, 7, 6, 5, 4, 3, 2] applied to all 8 digits." -> This implies the last digit has no weight or weight 1?
                               // Usually Denmark CVR is weighted sum % 11 == 0.
                               // Denmark weights are 2, 7, 6, 5, 4, 3, 2, 1.
                               // Let's assume the user meant [2, 7, 6, 5, 4, 3, 2, 1] or the last digit is included in the sum check.
                               // The user said: "Weights: [2, 7, 6, 5, 4, 3, 2] applied to all 8 digits."
                               // This is ambiguous. 7 weights for 8 digits.
                               // Let's assume the standard Modulo 11 check where the last digit makes the sum divisible by 11.
                               // If I use weights 2,7,6,5,4,3,2 for first 7 digits, then the remainder + last digit * 1 should be 0 mod 11?
                               // Let's check Denmark logic.
                               // DenmarkVatValidator usually uses weights { 2, 7, 6, 5, 4, 3, 2, 1 }.
                               // So I will use { 2, 7, 6, 5, 4, 3, 2, 1 }.

        int[] fullWeights = { 2, 7, 6, 5, 4, 3, 2, 1 };
        int fullSum = 0;
        for (int i = 0; i < 8; i++)
        {
            fullSum += (digits[i] - '0') * fullWeights[i];
        }

        if (fullSum % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, "Invalid checksum.");
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Greenland CVR number.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateCvr(number).IsValid)
        {
            throw new ArgumentException("Invalid CVR", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Greenland CVR number.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("GL"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
