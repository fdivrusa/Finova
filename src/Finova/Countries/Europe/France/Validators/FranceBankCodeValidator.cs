using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validates French Bank Code (Code Banque).
/// Format: 5 digits.
/// </summary>
public class FranceBankCodeValidator : IBankRoutingValidator
{
    /// <inheritdoc/>
    public string CountryCode => "FR";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "");

        if (sanitized.Length != 5)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "French Bank Code must be 5 digits.");
        }

        if (!Regex.IsMatch(sanitized, @"^\d{5}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "French Bank Code must contain only digits.");
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input)) return null;
        string sanitized = input.Replace(" ", "");
        return Validate(sanitized).IsValid ? sanitized : null;
    }
}
