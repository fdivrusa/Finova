using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Finland.Validators;

/// <summary>
/// Validator for Finland Personal Identity Code (Henkilötunnus).
/// </summary>
public class FinlandHenkilotunnusValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "FI";

    private const string CheckChars = "0123456789ABCDEFHJKLMNPRSTUVWXY";

    /// <summary>
    /// Validates the Finnish Henkilötunnus.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Finnish Henkilötunnus (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Normalize: Remove spaces, uppercase
        string normalized = nationalId.Trim().ToUpperInvariant();

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Check century sign
        char centurySign = normalized[6];
        if (centurySign != '+' && centurySign != '-' && centurySign != 'A')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid century sign.");
        }

        // Extract date and individual number
        string dateAndIndividual = normalized.Substring(0, 6) + normalized.Substring(7, 3);
        
        if (!long.TryParse(dateAndIndividual, out long number))
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

        // Calculate Checksum (Mod 31)
        int remainder = (int)(number % 31);
        char expectedCheckChar = CheckChars[remainder];

        if (normalized[10] != expectedCheckChar)
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

        string normalized = input.Trim().ToUpperInvariant();
        return normalized;
    }
}
