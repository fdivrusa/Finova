using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.China.Validators;

/// <summary>
/// Validates Chinese Resident Identity Card (RIC) numbers.
/// </summary>
public class ChinaResidentIdentityCardValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CN";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateStatic(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Trim().ToUpperInvariant();
        }
        return null;
    }

    /// <summary>
    /// Validates a Chinese Resident Identity Card number (Static).
    /// </summary>
    /// <param name="idCard">The ID card number (18 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idCard)
    {
        return ValidateRic(idCard);
    }

    /// <summary>
    /// Validates a Chinese Resident Identity Card number.
    /// </summary>
    /// <param name="idCard">The ID card number (18 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateRic(string? idCard)
    {
        if (string.IsNullOrWhiteSpace(idCard))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IdCardCannotBeEmpty);
        }

        var clean = idCard.Trim().ToUpperInvariant();

        if (clean.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.IdCardMustBe18Characters);
        }

        // First 17 must be digits
        if (!clean.Substring(0, 17).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.First17CharactersMustBeDigits);
        }

        // Last char can be digit or 'X'
        char lastChar = clean[17];
        if (!char.IsDigit(lastChar) && lastChar != 'X')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.LastCharacterMustBeDigitOrX);
        }

        // ISO 7064:1983, MOD 11-2
        int[] weights = { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
        char[] checkDigits = { '1', '0', 'X', '9', '8', '7', '6', '5', '4', '3', '2' };

        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        char expectedCheckDigit = checkDigits[remainder];

        if (lastChar != expectedCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidIdCardChecksum);
        }

        return ValidationResult.Success();
    }
}
