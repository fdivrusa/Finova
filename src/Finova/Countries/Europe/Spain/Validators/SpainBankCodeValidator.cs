using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validates Spanish Bank Code (Código de Entidad).
/// Format: 4 digits.
/// </summary>
public class SpainBankCodeValidator : IBankRoutingValidator
{
    /// <inheritdoc/>
    public string CountryCode => "ES";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "");

        if (sanitized.Length != 4)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Spanish Bank Code must be 4 digits.");
        }

        if (!Regex.IsMatch(sanitized, @"^\d{4}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Spanish Bank Code must contain only digits.");
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
