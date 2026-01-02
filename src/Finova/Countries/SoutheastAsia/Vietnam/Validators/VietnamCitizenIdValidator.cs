using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SoutheastAsia.Vietnam.Validators;

/// <summary>
/// Validates Vietnam Citizen Identity Card (CCCD) numbers.
/// </summary>
public class VietnamCitizenIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "VN";

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
            return input?.Trim().Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Vietnam Citizen Identity Card number.
    /// </summary>
    /// <param name="idNumber">The ID number (12 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace(" ", "");

        if (clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Structure: PPP G YY NNNNNN
        // PPP: Province code (001-096)
        // G: Gender/Century (0-9)
        // YY: Year of birth (00-99)
        // NNNNNN: Random

        return ValidationResult.Success();
    }
}
