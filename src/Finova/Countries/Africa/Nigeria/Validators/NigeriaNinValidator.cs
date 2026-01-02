using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Nigeria.Validators;

/// <summary>
/// Validates Nigerian National Identification Number (NIN).
/// </summary>
public class NigeriaNinValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "NG";

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
    /// Validates a Nigerian NIN.
    /// </summary>
    /// <param name="nin">The NIN (11 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nin)
    {
        if (string.IsNullOrWhiteSpace(nin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nin.Trim().Replace(" ", "");

        if (clean.Length != 11)
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
