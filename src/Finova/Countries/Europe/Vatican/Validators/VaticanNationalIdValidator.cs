using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Vatican.Validators;

public class VaticanNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "VA";

    /// <summary>
    /// Validates the Vatican National ID.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Vatican National ID (Static).
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? nationalId)
    {
        // Since there is no standard public format, we will return failure for now
        // or maybe just check for non-empty.
        // Let's assume it's not supported for now to be safe.
        return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Vatican City National ID validation is not supported.");
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return null;
    }
}
