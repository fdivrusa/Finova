using System;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validates the Netherlands National ID (Burgerservicenummer - BSN).
/// </summary>
public class NetherlandsNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "NL";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? InputSanitizer.Sanitize(input) : null;
    }

    /// <summary>
    /// Validates a Dutch BSN.
    /// </summary>
    /// <param name="bsn">The BSN to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? bsn)
    {
        if (string.IsNullOrWhiteSpace(bsn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string? sanitized = InputSanitizer.Sanitize(bsn);
        if (string.IsNullOrEmpty(sanitized))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // BSN must be 8 or 9 digits
        if (sanitized.Length < 8 || sanitized.Length > 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Pad with leading zero if length is 8
        if (sanitized.Length == 8)
        {
            sanitized = "0" + sanitized;
        }

        // Must contain only digits
        foreach (char c in sanitized)
        {
            if (!char.IsDigit(c))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
            }
        }

        // 11-test (Elfproef)
        // Sum = d1*9 + d2*8 + ... + d8*2 + d9*-1
        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (sanitized[i] - '0') * (9 - i);
        }

        sum += (sanitized[8] - '0') * -1;

        if (sum % 11 != 0)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
