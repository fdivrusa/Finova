using Finova.Core.Common;
using Finova.Core.Identifiers;
using System.Text.RegularExpressions;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validates the Spanish National ID (DNI) and Foreigner ID (NIE).
/// </summary>
public class SpainNationalIdValidator : INationalIdValidator
{
    private const string ControlLetters = "TRWAGMYFPDXBNJZSQVHLCKE";

    /// <inheritdoc/>
    public string CountryCode => "ES";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates the Spanish DNI/NIE format and checksum.
    /// </summary>
    /// <param name="input">The DNI or NIE to validate.</param>
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

        // Determine if DNI or NIE
        char firstChar = sanitized[0];
        string numberPart;

        if (char.IsDigit(firstChar))
        {
            // DNI: 8 digits + 1 letter
            if (!Regex.IsMatch(sanitized, @"^\d{8}[A-Z]$"))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }
            numberPart = sanitized.Substring(0, 8);
        }
        else if (firstChar == 'X' || firstChar == 'Y' || firstChar == 'Z')
        {
            // NIE: X/Y/Z + 7 digits + 1 letter
            if (!Regex.IsMatch(sanitized, @"^[XYZ]\d{7}[A-Z]$"))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }

            // Replace X->0, Y->1, Z->2
            string prefix = firstChar == 'X' ? "0" : (firstChar == 'Y' ? "1" : "2");
            numberPart = prefix + sanitized.Substring(1, 7);
        }
        else
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        if (!long.TryParse(numberPart, out long number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        int index = (int)(number % 23);
        char expectedLetter = ControlLetters[index];
        char actualLetter = sanitized[8];

        if (expectedLetter != actualLetter)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return InputSanitizer.Sanitize(input);
    }
}
