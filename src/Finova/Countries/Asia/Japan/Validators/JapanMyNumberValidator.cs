using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Asia.Japan.Validators;

/// <summary>
/// Validates Japanese My Number (Individual Number).
/// </summary>
public class JapanMyNumberValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "JP";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <summary>
    /// Validates a Japanese My Number.
    /// </summary>
    /// <param name="myNumber">The My Number string (12 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? myNumber)
    {
        if (string.IsNullOrWhiteSpace(myNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = InputSanitizer.Sanitize(myNumber);
        if (string.IsNullOrEmpty(clean))
        {
             return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidJapanMyNumberLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidJapanMyNumberFormat);
        }

        // Weights from left to right (for first 11 digits):
        // 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2
        int[] weights = { 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
        int sum = 0;

        for (int i = 0; i < 11; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder <= 1 ? 0 : 11 - remainder;

        if (checkDigit != (clean[11] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidJapanMyNumberChecksum);
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
