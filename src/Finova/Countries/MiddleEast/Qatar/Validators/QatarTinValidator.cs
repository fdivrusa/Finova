using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.MiddleEast.Qatar.Validators;

/// <summary>
/// Validator for Qatar Tax Identification Number (TIN).
/// Format: 10 digits.
/// </summary>
public class QatarTinValidator : ITaxIdValidator
{
    public string CountryCode => "QA";

    public ValidationResult Validate(string? input) => ValidateTin(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateTin(string? tin)
    {
        if (string.IsNullOrWhiteSpace(tin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = tin.Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "Qatar TIN must be 10 digits.");
        }

        return ValidationResult.Success();
    }
}