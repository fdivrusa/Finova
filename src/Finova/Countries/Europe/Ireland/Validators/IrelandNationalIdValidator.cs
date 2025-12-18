using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validates the Irish Personal Public Service Number (PPSN).
/// </summary>
public class IrelandNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "IE";

    private static readonly int[] Weights = { 8, 7, 6, 5, 4, 3, 2 };

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <summary>
    /// Validates the Irish Personal Public Service Number (PPSN) (Static).
    /// </summary>
    /// <param name="input">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
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

        // Length: 8 or 9 characters
        if (sanitized.Length < 8 || sanitized.Length > 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // First 7 must be digits
        string digitsPart = sanitized.Substring(0, 7);
        if (!long.TryParse(digitsPart, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "First 7 characters must be digits.");
        }

        // 8th char must be a letter (Check Character)
        char checkChar = sanitized[7];
        if (!char.IsLetter(checkChar))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "8th character must be a letter.");
        }

        // Optional 9th char must be a letter (W, T, X, etc.)
        char? lastChar = sanitized.Length == 9 ? sanitized[8] : null;
        if (lastChar.HasValue && !char.IsLetter(lastChar.Value))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "9th character must be a letter.");
        }

        // Calculate Sum
        int sum = 0;
        for (int i = 0; i < 7; i++)
        {
            sum += (digitsPart[i] - '0') * Weights[i];
        }

        // If 9 chars, add 9 * value of last char
        if (lastChar.HasValue)
        {
            sum += 9 * GetLetterValue(lastChar.Value);
        }

        int remainder = sum % 23;
        char expectedCheckChar = GetCheckChar(remainder);

        if (expectedCheckChar != checkChar)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    private static int GetLetterValue(char c)
    {
        // A=1, B=2, ..., W=23? No, usually W is 0 in this specific algo?
        // Actually, for the *input* letter (9th char), it's usually A=1..Z=26.
        // But for the *remainder* mapping, it's specific.
        // Let's assume standard A=1 offset for calculation.
        return c - 'A' + 1;
    }

    private static char GetCheckChar(int remainder)
    {
        // 0=W, 1=A, 2=B ...
        if (remainder == 0) return 'W';
        return (char)('A' + remainder - 1);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
