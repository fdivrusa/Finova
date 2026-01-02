using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SoutheastAsia.Thailand.Validators;

/// <summary>
/// Validates Thai National ID Card numbers.
/// </summary>
public class ThailandIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "TH";

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
            return input?.Trim().Replace("-", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Thai National ID Card number.
    /// </summary>
    /// <param name="idNumber">The ID number (13 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace("-", "").Replace(" ", "");

        if (clean.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Checksum Validation (Modulo 11)
        // Weights: 13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2
        int sum = 0;
        for (int i = 0; i < 12; i++)
        {
            sum += (clean[i] - '0') * (13 - i);
        }

        int remainder = sum % 11;
        int checkDigit = (11 - remainder) % 10;

        if (checkDigit != (clean[12] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
