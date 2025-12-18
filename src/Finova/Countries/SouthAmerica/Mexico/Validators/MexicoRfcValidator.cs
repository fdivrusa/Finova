using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Mexico.Validators;

/// <summary>
/// Validates Mexican RFC (Registro Federal de Contribuyentes).
/// </summary>
public partial class MexicoRfcValidator : ITaxIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "MX";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? input?.ToUpperInvariant().Replace(" ", "").Replace("-", "") : null;
    }

    // Regex for Person (13 chars) and Company (12 chars)
    // Person: 4 letters, 6 digits (YYMMDD), 3 alphanumeric (Homoclave)
    // Company: 3 letters, 6 digits (YYMMDD), 3 alphanumeric (Homoclave)
    [GeneratedRegex(@"^[A-ZÑ&]{3,4}\d{6}[A-Z0-9]{3}$")]
    private static partial Regex RfcRegex();

    /// <summary>
    /// Validates a Mexican RFC.
    /// </summary>
    /// <param name="rfc">The RFC string.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? rfc)
    {
        if (string.IsNullOrWhiteSpace(rfc))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = rfc.ToUpperInvariant().Replace(" ", "").Replace("-", "");

        if (clean.Length != 12 && clean.Length != 13)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidRfcLength);
        }

        if (!RfcRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidRfcFormat);
        }

        // Date validation could be added here (digits 4-9 or 3-8)
        // But basic regex covers the structure.

        return ValidationResult.Success();
    }
}
