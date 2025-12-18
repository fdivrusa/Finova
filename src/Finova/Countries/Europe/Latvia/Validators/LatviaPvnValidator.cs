using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Latvia.Validators;

/// <summary>
/// Validator for Latvia PVN (VAT Number).
/// Format: 11 digits.
/// </summary>
public partial class LatviaPvnValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "LV";

    public ValidationResult Validate(string? instance) => ValidatePvn(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Latvia PVN.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidatePvn(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove "LV" prefix if present and spaces
        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("LV"))
        {
            cleaned = cleaned[2..];
        }

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLatviaPvnLength);
        }

        // Weights: [9, 1, 4, 8, 3, 10, 2, 5, 7, 6] applied to first 10 digits.
        int[] weights = { 9, 1, 4, 8, 3, 10, 2, 5, 7, 6 };
        int sum = 0;

        for (int i = 0; i < 10; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        // Formula: R = Sum % 11. CheckDigit = (3 - R). If CheckDigit < -1, CheckDigit += 11.
        int remainder = sum % 11;
        int checkDigit = 3 - remainder;

        if (checkDigit < -1)
        {
            checkDigit += 11;
        }

        // Wait, spec says: "If CheckDigit < -1, add 13 to Result." -> "add 13 to Result" is weird if modulo 11.
        // "CheckDigit = (11 + 3 - (Sum % 11)) % 11" -> This handles the negative wrap around correctly.
        // Let's use the user's "proven Formula": R = Sum % 11. CheckDigit = (3 - R). If CheckDigit < -1, CheckDigit += 11.
        // Example: R=4. CD = 3-4 = -1. Valid? -1 is >= -1. So CD is -1? No digit can be -1.
        // "If Result < -1". So -1 is not < -1.
        // If R=4, CD=-1. If CD is -1, what is the digit?
        // Let's look at the "Correction for implementation": "If Remainder == 3, CheckDigit should be 0. If Remainder != 3, CheckDigit should be 3 - Remainder (handle negatives by adding 11)."
        // If R=3, CD=0.
        // If R=4, CD=3-4=-1. Add 11 -> 10? Digit cannot be 10.
        // If R=0, CD=3.
        // If R=1, CD=2.
        // If R=2, CD=1.
        // If R=5, CD=-2. Add 11 -> 9.
        // If R=10, CD=-7. Add 11 -> 4.

        // Is it possible the user meant "add 11" to make it positive modulo 11?
        // Let's assume the check digit must be 0-9.
        // If calculation results in 10 or -1 (which becomes 10), then it's invalid?
        // Or maybe the formula is: CheckDigit = (3 - R + 11) % 11.
        // If R=4, (3-4+11)%11 = 10. Digit 10 is impossible.
        // Maybe the weights or formula are slightly different for legal entities vs individuals?
        // "Latvia PVN (VAT Number)" -> Usually 11 digits.
        // Standard algorithm:
        // 1. Calculate weighted sum.
        // 2. R = Sum % 11.
        // 3. If R == 3, CheckDigit = 0.
        // 4. If R != 3, CheckDigit = 3 - R.
        //    If CheckDigit < 0, CheckDigit += 11. (This handles -1 to -7).
        //    But what if CheckDigit is 10? (e.g. R=4 -> CD=10).
        //    Usually this means the number is invalid (cannot be generated).

        // Let's implement exactly as requested:
        // "Use this proven Formula: R = Sum % 11. CheckDigit = (3 - R). If CheckDigit < -1, CheckDigit += 11. Compare with 11th digit."
        // If CheckDigit is -1, it stays -1? And comparison fails?
        // If CheckDigit is 10 (from -1+11), comparison fails?
        // This implies valid numbers won't result in -1 or 10.

        if (checkDigit < -1)
        {
            checkDigit += 11;
        }

        int lastDigit = digits[10] - '0';
        if (checkDigit != lastDigit)
        {
            // Special case: maybe -1 maps to something?
            // But if checkDigit is -1 or 10, it will never match lastDigit (0-9).
            // So this correctly invalidates those cases.
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Latvia PVN.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidatePvn(number).IsValid)
        {
            throw new ArgumentException("Invalid PVN", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Latvia PVN.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("LV"))
        {
            cleaned = cleaned[2..];
        }

        return DigitsOnlyRegex().Replace(cleaned, "");
    }
}
