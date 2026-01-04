using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Belgium.Validators;

/// <summary>
/// Validator for Belgian BBAN (Basic Bank Account Number).
/// Format: 3-digit bank code + 7-digit account number + 2-digit check digits = 12 digits total.
/// Often displayed as XXX-XXXXXXX-YY.
/// </summary>
public class BelgiumBbanValidator : IBbanValidator
{
    public string CountryCode => "BE";
    ValidationResult IValidator<string>.Validate(string? input) => Validate(input);

    /// <summary>
    /// Validates the Belgian BBAN structure and checksum.
    /// Format: 3 Bank + 7 Account + 2 National Check (Total 12 digits).
    /// Algorithm: First10 % 97 == Last2.
    /// </summary>
    /// <param name="bban">The BBAN string (12 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Sanitize input (remove spaces, dashes, etc.)
        bban = InputSanitizer.Sanitize(bban);

        if (string.IsNullOrWhiteSpace(bban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (bban.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 12, bban.Length));
        }

        // Ensure all characters are digits
        foreach (char c in bban)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
            }
        }

        // Split into Data (10 digits) and Check (2 digits)
        // Using ReadOnlySpan for performance
        ReadOnlySpan<char> bbanSpan = bban.AsSpan();
        ReadOnlySpan<char> dataPart = bbanSpan[..10];
        ReadOnlySpan<char> checkPart = bbanSpan[10..];

        if (!long.TryParse(dataPart, out long dataValue) ||
            !int.TryParse(checkPart, out int checkValue))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // Calculate Modulo
        long remainder = dataValue % 97;

        // Specific rule: If remainder is 0, the check digits must be 97
        bool isValid;
        if (remainder == 0)
        {
            isValid = checkValue == 97;
        }
        else
        {
            isValid = remainder == checkValue;
        }

        return isValid
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidBelgiumBbanStructure);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "").Replace("-", "").Trim();
        return Validate(sanitized).IsValid ? sanitized : null;
    }

    /// <inheritdoc/>
    public BbanDetails? ParseDetails(string? bban)
    {
        if (string.IsNullOrWhiteSpace(bban)) return null;

        string sanitized = bban.Replace(" ", "").Replace("-", "").Trim();

        if (!Validate(sanitized).IsValid) return null;

        // Belgian BBAN: 3-digit bank code + 7-digit account + 2-digit check
        return new BbanDetails
        {
            Bban = sanitized,
            CountryCode = CountryCode,
            BankCode = sanitized[..3],           // Code Banque (3 digits)
            BranchCode = null,                    // Belgium doesn't have separate branch codes
            AccountNumber = sanitized[3..10],     // Account number (7 digits)
            NationalCheckDigits = sanitized[10..] // Clef de contrôle (2 digits)
        };
    }
}
