using System;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Sweden.Validators;

/// <summary>
/// Validator for Sweden Personal Identity Number (Personnummer).
/// Format: YYYYMMDD-XXXX (12 digits) or YYMMDD-XXXX (10 digits).
/// </summary>
public class SwedenNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "SE";

    /// <summary>
    /// Validates the Sweden Personnummer.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId) => ValidateStatic(nationalId);

    /// <summary>
    /// Validates the Sweden Personnummer (Static).
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

        // Length check: 10 or 12 digits
        if (sanitized.Length != 10 && sanitized.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!long.TryParse(sanitized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        // If 12 digits, ignore first 2 (century) for checksum
        string toCheck = sanitized.Length == 12 ? sanitized.Substring(2) : sanitized;

        // Luhn Algorithm
        if (!ChecksumHelper.ValidateLuhn(toCheck))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
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
