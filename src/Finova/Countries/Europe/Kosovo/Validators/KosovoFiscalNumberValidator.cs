using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Kosovo.Validators;

/// <summary>
/// Validator for Kosovo Fiscal Number (Numri Fiskal).
/// Format: 9 digits.
/// </summary>
public partial class KosovoFiscalNumberValidator : ITaxIdValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    public string CountryCode => "XK";

    public ValidationResult Validate(string? instance) => ValidateFiscalNumber(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Kosovo Fiscal Number.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateFiscalNumber(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.EnterpriseNumberCannotBeEmpty);
        }

        // Remove spaces
        var cleaned = number.Trim().ToUpperInvariant();

        var digits = DigitsOnlyRegex().Replace(cleaned, "");

        if (digits.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidKosovoFiscalNumberLength);
        }

        // Weights: [9, 8, 7, 6, 5, 4, 3, 2] applied to first 8 digits.
        int[] weights = { 9, 8, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            sum += (digits[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 10)
        {
            checkDigit = 0; // Spec says: If Remainder == 10, CheckDigit = 0.
        }
        else
        {
            checkDigit = remainder; // Else, CheckDigit = Remainder.
        }

        int lastDigit = digits[8] - '0';
        if (checkDigit != lastDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Kosovo Fiscal Number.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateFiscalNumber(number).IsValid)
        {
            throw new ArgumentException("Invalid Fiscal Number", nameof(number));
        }

        return Normalize(number);
    }

    /// <summary>
    /// Normalizes a Kosovo Fiscal Number.
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
