using Finova.Core.Common;

namespace Finova.Countries.Asia.Japan.Validators;

/// <summary>
/// Validates Japanese Corporate Number.
/// </summary>
public static class JapanCorporateNumberValidator
{
    /// <summary>
    /// Validates a Japanese Corporate Number.
    /// </summary>
    /// <param name="corporateNumber">The Corporate Number string (13 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? corporateNumber)
    {
        if (string.IsNullOrWhiteSpace(corporateNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = corporateNumber.Replace(" ", "").Replace("-", "");

        if (clean.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidJapanCorporateNumberLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidJapanCorporateNumberFormat);
        }

        // Calculate checksum using Mod 9 algorithm
        // Weights for digits at indices 1 to 12 alternate between 1 and 2
        int sum = 0;
        for (int i = 1; i <= 12; i++)
        {
            int digit = clean[i] - '0';
            int weight = (i % 2 == 0) ? 2 : 1; 
            sum += digit * weight;
        }

        int remainder = sum % 9;
        int checkDigit = 9 - remainder;

        if (checkDigit != (clean[0] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidJapanCorporateNumberChecksum);
        }

        return ValidationResult.Success();
    }
}
