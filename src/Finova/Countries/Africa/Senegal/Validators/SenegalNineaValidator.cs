using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Senegal.Validators;

/// <summary>
/// Validator for Senegal NINEA (Numéro d'Identification Nationale des Entreprises et des Associations).
/// Format: 15 digits.
/// </summary>
public class SenegalNineaValidator : ITaxIdValidator
{
    public string CountryCode => "SN";

    public ValidationResult Validate(string? input) => ValidateNinea(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateNinea(string? ninea)
    {
        if (string.IsNullOrWhiteSpace(ninea))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = ninea.Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSenegalNineaLength);
        }

        return ValidationResult.Success();
    }
}