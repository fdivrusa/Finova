using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Hungary.Validators;

/// <summary>
/// Validator for Hungary Adószám (Tax ID).
/// Format: 11 digits (Format 12345678-S-CC).
/// </summary>
public partial class HungaryAdoszamValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "HU";

    public ValidationResult Validate(string? instance) => ValidateAdoszam(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Hungary Adószám.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateAdoszam(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove hyphens and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        // No country prefix specified to strip, but let's be safe if HU is passed? 
        // User said: "Sanitization: Remove hyphens, spaces. Must be 11 digits." -> No HU prefix mentioned.
        // But Normalize should strip country prefixes usually. Let's stick to user spec: "Remove hyphens, spaces".

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidHungaryAdoszamLength);
        }

        // Focus validation only on the first 8 digits (the core "Törzsszám").
        // Weights: [9, 7, 3, 1, 9, 7, 3] applied to first 7 digits.
        int[] weights = { 9, 7, 3, 1, 9, 7, 3 };
        int sum = 0;

        for (int i = 0; i < 7; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 10;
        int checkDigit = (10 - remainder) % 10;

        int eighthDigit = digits[7] - '0';
        if (checkDigit != eighthDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Hungary Adószám.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateAdoszam(number).IsValid)
        {
            throw new ArgumentException("Invalid Adószám", nameof(number));
        }

        var normalized = Normalize(number);
        // Format: 12345678-S-CC
        return $"{normalized.Substring(0, 8)}-{normalized.Substring(8, 1)}-{normalized.Substring(9, 2)}";
    }

    /// <summary>
    /// Normalizes a Hungary Adószám.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        return DigitsOnlyRegex().Replace(number, "");
    }
}
