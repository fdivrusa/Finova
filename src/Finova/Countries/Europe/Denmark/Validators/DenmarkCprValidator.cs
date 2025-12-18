using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Denmark.Validators;

/// <summary>
/// Validator for Denmark Personal Identification Number (CPR).
/// </summary>
public class DenmarkCprValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "DK";

    private static readonly int[] Weights = { 4, 3, 2, 7, 6, 5, 4, 3, 2, 1 };

    /// <summary>
    /// Validates the Danish CPR number.
    /// </summary>
    /// <param name="nationalId">The CPR to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Danish CPR number (Static).
    /// </summary>
    /// <param name="nationalId">The CPR to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Normalize: Remove spaces, hyphens
        string normalized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        if (normalized.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(normalized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Date
        int day = int.Parse(normalized.Substring(0, 2));
        int month = int.Parse(normalized.Substring(2, 2));

        if (month < 1 || month > 12 || day < 1 || day > 31)
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid date part.");
        }

        // Mod 11 Checksum
        // Note: Since 2007, CPR numbers without modulus 11 check exist, but they are rare/specific.
        // Standard validation usually requires Mod 11.
        // If the remainder is 0, it's valid.
        
        int sum = 0;
        for (int i = 0; i < 10; i++)
        {
            sum += (normalized[i] - '0') * Weights[i];
        }

        if (sum % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return null;
        }

        return InputSanitizer.Sanitize(input);
    }
}
