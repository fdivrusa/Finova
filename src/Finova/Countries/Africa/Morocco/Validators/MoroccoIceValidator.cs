using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Morocco.Validators;

/// <summary>
/// Validator for Moroccan ICE (Identifiant Commun de l'Entreprise).
/// Format: 15 digits.
/// </summary>
public class MoroccoIceValidator : ITaxIdValidator
{
    public string CountryCode => "MA";

    public ValidationResult Validate(string? input) => ValidateIce(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim() : null;

    public static ValidationResult ValidateIce(string? ice)
    {
        if (string.IsNullOrWhiteSpace(ice))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = ice.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("MA", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidMoroccoIceLength);
        }

        // ICE format: 9 digits (RC sequence) + 2 digits (RC city) + 4 digits (check/control)
        // Currently no public checksum algorithm is available, validating format only.
        return ValidationResult.Success();
    }
}