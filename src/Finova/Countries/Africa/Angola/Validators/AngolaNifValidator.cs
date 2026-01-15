using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Angola.Validators;

/// <summary>
/// Validator for Angola NIF (Número de Identificação Fiscal).
/// Format: 9 or 10 digits.
/// </summary>
public class AngolaNifValidator : ITaxIdValidator
{
    public string CountryCode => "AO";

    public ValidationResult Validate(string? input) => ValidateNif(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateNif(string? nif)
    {
        if (string.IsNullOrWhiteSpace(nif))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nif.Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 9 && clean.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidAngolaNifLength);
        }

        return ValidationResult.Success();
    }
}