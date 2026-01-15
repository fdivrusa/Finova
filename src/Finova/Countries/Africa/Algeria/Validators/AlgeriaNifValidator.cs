using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Algeria.Validators;

/// <summary>
/// Validator for Algerian NIF (Numéro d'Identification Fiscale).
/// Format: 15 digits (20 digits for older online IDs). Standard is 15 digits.
/// </summary>
public class AlgeriaNifValidator : ITaxIdValidator
{
    public string CountryCode => "DZ";

    public ValidationResult Validate(string? input) => ValidateNif(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateNif(string? nif)
    {
        if (string.IsNullOrWhiteSpace(nif))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = nif.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("DZ", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 15 && clean.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidAlgeriaNifLength);
        }

        return ValidationResult.Success();
    }
}