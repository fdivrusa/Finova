using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom National Insurance Number (NINO).
/// Format: AA 12 34 56 A.
/// </summary>
public class UnitedKingdomNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "GB";

    /// <summary>
    /// Validates the UK NINO.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId) => ValidateStatic(nationalId);

    /// <summary>
    /// Validates the UK NINO (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = InputSanitizer.Sanitize(nationalId) ?? string.Empty;

        if (sanitized.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Format: 2 letters, 6 digits, 1 letter
        if (!Regex.IsMatch(sanitized, "^[A-Z]{2}[0-9]{6}[A-Z]$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Specific invalid prefixes
        string prefix = sanitized.Substring(0, 2);
        string[] invalidPrefixes = { "BG", "GB", "NK", "KN", "TN", "NT", "ZZ" };
        foreach (var invalid in invalidPrefixes)
        {
            if (prefix == invalid)
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
            }
        }

        // Second letter cannot be O
        if (sanitized[1] == 'O')
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }
}
