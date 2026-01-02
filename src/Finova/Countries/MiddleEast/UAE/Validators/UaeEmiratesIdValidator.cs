using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.UAE.Validators;

/// <summary>
/// Validates UAE Emirates ID numbers.
/// </summary>
public class UaeEmiratesIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AE";

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
    /// Validates a UAE Emirates ID number.
    /// </summary>
    /// <param name="idNumber">The ID number (15 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace("-", "").Replace(" ", "");

        if (clean.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        if (!clean.StartsWith("784"))
        {
            // Emirates ID usually starts with 784 (UAE country code).
            // However, some sources say it might change? Let's be strict for now.
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Validate Checksum (Luhn)
        if (!ChecksumHelper.ValidateLuhn(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}
