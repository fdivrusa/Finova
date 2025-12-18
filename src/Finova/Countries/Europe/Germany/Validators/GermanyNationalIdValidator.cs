using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validates the German Identity Card Number (Personalausweisnummer).
/// </summary>
public class GermanyNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "DE";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the German Identity Card Number format and checksum.
    /// Format: 9 alphanumeric characters.
    /// Checksum: The last character is a check digit calculated from the first 8 characters using weights 7, 3, 1.
    /// </summary>
    /// <param name="input">The Identity Card Number to validate.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult ValidateStatic(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string? sanitized = InputSanitizer.Sanitize(input);
        if (string.IsNullOrEmpty(sanitized))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (sanitized.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Validate format: Alphanumeric
        foreach (char c in sanitized)
        {
            if (!char.IsLetterOrDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }
        }

        // Validate Checksum
        if (!ValidateChecksum(sanitized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    private static bool ValidateChecksum(string input)
    {
        // Weights: 7, 3, 1 repeating
        int[] weights = { 7, 3, 1 };
        int sum = 0;

        // Calculate sum for the first 8 characters
        for (int i = 0; i < 8; i++)
        {
            int value = GetCharValue(input[i]);
            int weight = weights[i % 3];
            sum += value * weight;
        }

        int calculatedCheckDigit = sum % 10;
        int actualCheckDigit = GetCharValue(input[8]);

        return calculatedCheckDigit == actualCheckDigit;
    }

    private static int GetCharValue(char c)
    {
        if (char.IsDigit(c))
        {
            return c - '0';
        }
        if (char.IsLetter(c))
        {
            return char.ToUpperInvariant(c) - 'A' + 10;
        }
        return 0; // Should not happen due to prior validation
    }
    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
