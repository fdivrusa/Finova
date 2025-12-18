using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Azerbaijan.Validators;

/// <summary>
/// Validator for Azerbaijan Personal Identification Number (PIN / FİN).
/// Format: 7 alphanumeric characters.
/// </summary>
public class AzerbaijanNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "AZ";

    /// <summary>
    /// Validates the Azerbaijan PIN.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Azerbaijan PIN (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        if (string.IsNullOrWhiteSpace(nationalId))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = nationalId.Trim().ToUpperInvariant();

        if (sanitized.Length != 7)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        // Must be alphanumeric
        if (!Regex.IsMatch(sanitized, "^[A-Z0-9]{7}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? input?.Trim().ToUpperInvariant() : null;
    }
}
