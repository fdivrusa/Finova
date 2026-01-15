using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.DominicanRepublic.Validators;

/// <summary>
/// Validator for Dominican Republic RNC (Registro Nacional de Contribuyentes).
/// Format: 9 or 11 digits.
/// </summary>
public class DominicanRepublicRncValidator : ITaxIdValidator
{
    public string CountryCode => "DO";

    public ValidationResult Validate(string? input) => ValidateRnc(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateRnc(string? rnc)
    {
        if (string.IsNullOrWhiteSpace(rnc))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = rnc.Replace("-", "").Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 9 && clean.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidDominicanRepublicRncLength);
        }

        return ValidationResult.Success();
    }
}