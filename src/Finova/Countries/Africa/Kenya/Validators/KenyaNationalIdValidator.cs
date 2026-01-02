using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Kenya.Validators;

/// <summary>
/// Validates Kenyan National ID numbers.
/// </summary>
public class KenyaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "KE";

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
    /// Validates a Kenyan National ID number.
    /// </summary>
    /// <param name="idNumber">The ID number (7-9 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? idNumber)
    {
        if (string.IsNullOrWhiteSpace(idNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = idNumber.Trim().Replace(" ", "");

        if (clean.Length < 7 || clean.Length > 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }
}
