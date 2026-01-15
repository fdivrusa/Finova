using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Africa.Tunisia.Validators;

/// <summary>
/// Validator for Tunisian Matricule Fiscal.
/// Format: 7 or 8 digits + 1 letter (Control) + 1 letter (Category) + 1 letter (TVA) + 3 digits (Estab).
/// Example: 1234567A/B/M/000. Simplified: 7-8 digits + A-Z + A-Z + A-Z + 3 digits.
/// </summary>
public partial class TunisiaMatriculeFiscalValidator : ITaxIdValidator
{
    public string CountryCode => "TN";

    [GeneratedRegex(@"^\d{7,8}[A-Z][A-Z][A-Z]\d{3}$")]
    private static partial Regex MfRegex();

    public ValidationResult Validate(string? input) => ValidateMf(input);

    public string? Parse(string? input) => Validate(input).IsValid ? input?.Replace("/", "").Trim().ToUpperInvariant() : null;

    public static ValidationResult ValidateMf(string? mf)
    {
        if (string.IsNullOrWhiteSpace(mf))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = mf.Replace("/", "").Replace(" ", "").Replace("-", "").Trim().ToUpperInvariant();

        if (clean.StartsWith("TN", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!MfRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTunisiaMfFormat);
        }

        return ValidationResult.Success();
    }
}