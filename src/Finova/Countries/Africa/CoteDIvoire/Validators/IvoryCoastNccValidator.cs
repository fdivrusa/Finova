using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.CoteDIvoire.Validators;

/// <summary>
/// Validator for Ivory Coast (Côte d'Ivoire) NCC (Numéro de Compte Contribuable).
/// Format: 7 digits + 1 control letter.
/// </summary>
public partial class IvoryCoastNccValidator : ITaxIdValidator
{
    public string CountryCode => "CI";

    [GeneratedRegex(@"^\d{7}[A-Z]$")]
    private static partial Regex NccRegex();

    public ValidationResult Validate(string? input) => ValidateNcc(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim().ToUpperInvariant() : null;

    public static ValidationResult ValidateNcc(string? ncc)
    {
        if (string.IsNullOrWhiteSpace(ncc))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = ncc.Trim().ToUpperInvariant();

        if (!NccRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidIvoryCoastNccFormat);
        }

        return ValidationResult.Success();
    }
}