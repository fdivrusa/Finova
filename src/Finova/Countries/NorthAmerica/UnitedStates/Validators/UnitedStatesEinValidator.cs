using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.UnitedStates.Validators;

/// <summary>
/// Validates United States Employer Identification Numbers (EIN).
/// </summary>
public class UnitedStatesEinValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "US";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        return ValidateEin(input);
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (Validate(input).IsValid)
        {
            return input?.Replace("-", "").Replace(" ", "");
        }
        return null;
    }

    /// <summary>
    /// Validates a US Employer Identification Number (EIN).
    /// </summary>
    /// <param name="ein">The EIN string (e.g., "12-3456789").</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateEin(string? ein)
    {
        if (string.IsNullOrWhiteSpace(ein))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove separators
        var clean = ein.Replace("-", "").Replace(" ", "");

        if (clean.Length != 9)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidEinLength);
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidEinFormat);
        }

        // Validates format only. Prefix validation requires a lookup table.
        return ValidationResult.Success();
    }
}
