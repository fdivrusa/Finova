using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validates Italian ABI Code (Associazione Bancaria Italiana).
/// Format: 5 digits.
/// </summary>
public class ItalyBankCodeValidator : IBankRoutingValidator
{
    /// <inheritdoc/>
    public string CountryCode => "IT";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Italian ABI Code must be 5 digits.");
        }

        if (!Regex.IsMatch(sanitized, @"^\d{5}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Italian ABI Code must contain only digits.");
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
