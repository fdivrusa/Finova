using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Nicaragua.Validators;

/// <summary>
/// Validator for Nicaragua RUC (Registro Único de Contribuyentes).
/// Format: 14 characters.
/// </summary>
public class NicaraguaRucValidator : ITaxIdValidator
{
    public string CountryCode => "NI";

    public ValidationResult Validate(string? input) => ValidateRuc(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateRuc(string? ruc)
    {
        if (string.IsNullOrWhiteSpace(ruc))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = ruc.Replace("-", "").Trim();

        if (clean.Length != 14)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidNicaraguaRucLength);
        }

        return ValidationResult.Success();
    }
}