using System.Numerics;
using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validates the French National ID (Numéro de Sécurité Sociale / NIR).
/// </summary>
public class FranceNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "FR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the French National ID format and checksum.
    /// </summary>
    /// <param name="input">The NIR string to validate.</param>
    /// <returns>A <see cref="ValidationResult"/>.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 1. Sanitize: Remove spaces, dots, dashes
        string? sanitized = InputSanitizer.Sanitize(input);

        if (string.IsNullOrEmpty(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // 2. Length Check: Must be 15 characters (13 digits + 2 key digits)
        if (sanitized.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "French National ID must be 15 characters long.");
        }

        // 3. Format Check: 13 chars (digits or 2A/2B) + 2 digits
        // Regex: First digit (1/2), then 12 digits OR specific Corsica pattern, then 2 digits key
        if (!Regex.IsMatch(sanitized, @"^[12]\d{14}$") && !Regex.IsMatch(sanitized, @"^[12]\d{4}(2A|2B)\d{6}\d{2}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid French National ID format.");
        }

        // 4. Checksum Calculation
        string numberPart = sanitized.Substring(0, 13);
        string keyPart = sanitized.Substring(13, 2);

        if (!long.TryParse(keyPart, out long key))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid key format.");
        }

        // Handle Corsica (2A -> 19, 2B -> 18) for calculation
        string numericString = numberPart;
        if (numberPart.Contains("2A"))
        {
            numericString = numberPart.Replace("2A", "19");
        }
        else if (numberPart.Contains("2B"))
        {
            numericString = numberPart.Replace("2B", "18");
        }

        if (!BigInteger.TryParse(numericString, out BigInteger number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid numeric content.");
        }

        // NIR Key Formula: 97 - (Number % 97)
        long calculatedKey = 97 - (long)(number % 97);

        if (calculatedKey != key)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = ValidateStatic(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
