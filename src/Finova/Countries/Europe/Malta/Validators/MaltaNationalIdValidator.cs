using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Malta.Validators;

/// <summary>
/// Validator for Malta Identity Card Number.
/// Format: 1 to 7 digits followed by a letter (A, B, G, H, L, M, P, Z).
/// </summary>
public class MaltaNationalIdValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "MT";

    /// <summary>
    /// Validates the Malta Identity Card Number.
    /// </summary>
    /// <param name="nationalId">The ID to validate.</param>
    /// <returns>A <see cref="ValidationResult"/> indicating success or failure.</returns>
    public ValidationResult Validate(string? nationalId)
    {
        return ValidateStatic(nationalId);
    }

    /// <summary>
    /// Validates the Malta Identity Card Number (Static).
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

        // Format: 1 to 7 digits followed by 1 letter.
        // Regex: ^\d{1,7}[ABGHLMPZ]$
        if (!Regex.IsMatch(sanitized, @"^\d{1,7}[ABGHLMPZ]$"))
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
