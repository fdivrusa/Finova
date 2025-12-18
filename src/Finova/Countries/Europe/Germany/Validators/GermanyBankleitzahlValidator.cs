using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Germany.Validators;

/// <summary>
/// Validates German Bankleitzahl (BLZ) - Bank Routing Number.
/// Format: 8 digits.
/// </summary>
public class GermanyBankleitzahlValidator : IBankRoutingValidator
{
    /// <inheritdoc/>
    public string CountryCode => "DE";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        string sanitized = input.Replace(" ", "");

        if (sanitized.Length != 8)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "German BLZ must be 8 digits.");
        }

        if (!Regex.IsMatch(sanitized, @"^\d{8}$"))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "German BLZ must contain only digits.");
        }

        return ValidationResult.Success();
    }

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        return Validate(input).IsValid ? input : null;
    }
}
