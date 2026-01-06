using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Honduras.Validators;

/// <summary>
/// Validator for Honduras RTN (Registro Tributario Nacional).
/// Format: 14 digits.
/// </summary>
public class HondurasRtnValidator : ITaxIdValidator
{
    public string CountryCode => "HN";

    public ValidationResult Validate(string? input) => ValidateRtn(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateRtn(string? rtn)
    {
        if (string.IsNullOrWhiteSpace(rtn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = rtn.Replace("-", "").Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "RTN must be 14 digits.");
        }

        return ValidationResult.Success();
    }
}