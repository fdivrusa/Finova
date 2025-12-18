using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Lithuania.Validators;

/// <summary>
/// Validator for Lithuania PVM (VAT) / Įmonės kodas.
/// Format: 9 digits (Entities).
/// </summary>
public partial class LithuaniaPvmValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "LT";

    public ValidationResult Validate(string? instance) => ValidatePvm(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Lithuania PVM / Entity Code.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidatePvm(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove "LT" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("LT"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidPvmLength);
        }

        // Pass 1: Weights [1, 2, 3, 4, 5, 6, 7, 8]
        int[] weights1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
        int sum1 = 0;
        for (int i = 0; i < 8; i++)
        {
            sum1 += (digits[i] - '0') * weights1[i];
        }

        int remainder1 = sum1 % 11;
        int checkDigit;

        if (remainder1 != 10)
        {
            checkDigit = remainder1;
        }
        else
        {
            // Pass 2: Weights [3, 4, 5, 6, 7, 8, 9, 1]
            int[] weights2 = { 3, 4, 5, 6, 7, 8, 9, 1 };
            int sum2 = 0;
            for (int i = 0; i < 8; i++)
            {
                sum2 += (digits[i] - '0') * weights2[i];
            }

            int remainder2 = sum2 % 11;
            if (remainder2 != 10)
            {
                checkDigit = remainder2;
            }
            else
            {
                checkDigit = 0;
            }
        }

        int lastDigit = digits[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Lithuania PVM.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidatePvm(number).IsValid)
        {
            throw new ArgumentException("Invalid PVM", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Lithuania PVM.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("LT"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
