using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.NorthAmerica.CostaRica.Validators;

/// <summary>
/// Validator for Costa Rica NITE (Número de Identificación Tributaria Especial).
/// Format: 10 or 12 digits.
/// </summary>
public class CostaRicaNiteValidator : ITaxIdValidator
{
    public string CountryCode => "CR";

    public ValidationResult Validate(string? input) => ValidateNite(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateNite(string? nite)
    {
        if (string.IsNullOrWhiteSpace(nite))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nite.Replace("-", "").Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 10 && clean.Length != 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidCostaRicaNiteLength);
        }

        return ValidationResult.Success();
    }
}