using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.Guatemala.Validators;

/// <summary>
/// Validator for Guatemala NIT (Número de Identificación Tributaria).
/// Format: Up to 12 characters.
/// </summary>
public class GuatemalaNitValidator : ITaxIdValidator
{
    public string CountryCode => "GT";

    public ValidationResult Validate(string? input) => ValidateNit(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateNit(string? nit)
    {
        if (string.IsNullOrWhiteSpace(nit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nit.Replace("-", "").Trim();

        if (clean.Length < 2 || clean.Length > 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "NIT must be between 2 and 12 characters.");
        }

        return ValidationResult.Success();
    }
}