using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.ElSalvador.Validators;

/// <summary>
/// Validator for El Salvador NIT (Número de Identificación Tributaria).
/// Format: 14 digits.
/// </summary>
public class ElSalvadorNitValidator : ITaxIdValidator
{
    public string CountryCode => "SV";

    public ValidationResult Validate(string? input) => ValidateNit(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateNit(string? nit)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nit.Replace("-", "").Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "NIT must be 14 digits.");
        }

        return ValidationResult.Success();
    }
}