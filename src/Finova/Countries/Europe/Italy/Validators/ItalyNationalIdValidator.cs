using System;
using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Internals;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian National ID (Codice Fiscale).
/// </summary>
public class ItalyNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "IT";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <inheritdoc/>
    public string? Parse(string? input) => InputSanitizer.Sanitize(input);

    private static readonly int[] OddDigitValues = { 1, 0, 5, 7, 9, 13, 15, 17, 19, 21 };

    private static readonly int[] OddLetterValues = {
        1, 0, 5, 7, 9, 13, 15, 17, 19, 21, // A-J
        2, 4, 18, 20, 11, 3, 6, 8, 12, 14, // K-T
        16, 10, 22, 25, 24, 23             // U-Z
    };

    /// <summary>
    /// Validates an Italian Codice Fiscale.
    /// </summary>
    /// <param name="codiceFiscale">The Codice Fiscale to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? codiceFiscale)
    {
        if (string.IsNullOrWhiteSpace(codiceFiscale))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string? sanitized = InputSanitizer.Sanitize(codiceFiscale);

        if (string.IsNullOrEmpty(sanitized))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (sanitized.Length != 16)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Format check:
        // First 15 chars are alphanumeric. Last char is letter.
        // Actually, usually: 6 letters, 2 digits, 1 letter, 2 digits, 1 letter, 3 digits, 1 letter.
        // But omocodia (homocodia) rules replace digits with letters.
        // So generally, it's 16 alphanumeric chars.
        // The last char MUST be a letter (Check digit).

        char checkDigit = sanitized[15];
        if (!char.IsLetter(checkDigit))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        int total = 0;
        for (int i = 0; i < 15; i++)
        {
            char c = sanitized[i];
            if (!char.IsLetterOrDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }

            bool isOddPosition = (i + 1) % 2 != 0; // 1-based index

            if (isOddPosition)
            {
                if (char.IsDigit(c))
                {
                    total += OddDigitValues[c - '0'];
                }
                else
                {
                    total += OddLetterValues[c - 'A'];
                }
            }
            else
            {
                // Even position
                // '0'-'9' -> 0-9
                // 'A'-'Z' -> 0-25
                if (char.IsDigit(c))
                {
                    total += c - '0';
                }
                else
                {
                    total += c - 'A';
                }
            }
        }

        int remainder = total % 26;
        char expectedCheckDigit = (char)('A' + remainder);

        if (checkDigit != expectedCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
