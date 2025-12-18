using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Greenland.Validators;

/// <summary>
/// Validator for Greenland CVR (Centrale Virksomhedsregister).
/// Format: 8 digits.
/// </summary>
public partial class GreenlandCvrValidator : ITaxIdValidator
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.CvrNumberMustBe8Digits);
        }

        // Weights: [2, 7, 6, 5, 4, 3, 2, 1]
        int[] fullWeights = { 2, 7, 6, 5, 4, 3, 2, 1 };
        int fullSum = 0;
        for (int i = 0; i < 8; i++)
        {
            fullSum += (digits[i] - '0') * fullWeights[i];
        }

        if (fullSum % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
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
