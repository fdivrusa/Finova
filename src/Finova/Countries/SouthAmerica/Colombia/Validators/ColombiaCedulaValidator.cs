using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Colombia.Validators;

/// <summary>
/// Validates Colombian Cédula de Ciudadanía numbers.
/// </summary>
public class ColombiaCedulaValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "CO";

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
            return input?.Trim().Replace(".", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a Colombian Cédula number.
    /// </summary>
    /// <param name="cedula">The Cédula number (6-10 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? cedula)
    {
        if (string.IsNullOrWhiteSpace(cedula))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = cedula.Trim().Replace(".", "").Replace(" ", "");

        if (clean.Length < 6 || clean.Length > 10)
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
