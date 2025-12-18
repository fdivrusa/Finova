using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegro PIB (Poreski Identifikacioni Broj).
/// Format: 8 digits.
/// </summary>
public partial class MontenegroPibValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "ME";

    public ValidationResult Validate(string? instance) => ValidatePib(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Montenegro PIB.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidatePib(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove spaces
        var cleaned = number.Trim().ToUpperInvariant();

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.PibMustBe8Digits);
        }



        int[] weights = { 7, 6, 5, 4, 3, 2, 1 };
        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else if (remainder == 1) // Spec says: "If Remainder == 1, Invalid."
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksumRemainder1);
        }
        else
        {
            checkDigit = 11 - remainder;
        }

        int lastDigit = digits[7] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Montenegro PIB.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidatePib(number).IsValid)
        {
            throw new ArgumentException("Invalid PIB", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Montenegro PIB.
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
