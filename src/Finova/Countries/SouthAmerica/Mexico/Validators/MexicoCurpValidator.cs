using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.SouthAmerica.Mexico.Validators;

/// <summary>
/// Validates Mexican CURP (Clave Única de Registro de Población).
/// </summary>
public partial class MexicoCurpValidator : INationalIdValidator
{
    /// <inheritdoc/>
    public string CountryCode => "MX";

    /// <inheritdoc/>
    public ValidationResult Validate(string? input) => ValidateStatic(input);

    /// <inheritdoc/>
    public string? Parse(string? input)
    {
        var result = Validate(input);
        return result.IsValid ? input?.ToUpperInvariant().Trim() : null;
    }

    [GeneratedRegex(@"^[A-Z]{4}\d{6}[HM][A-Z]{2}[B-DF-HJ-NP-TV-Z]{3}[A-Z0-9]\d$")]
    private static partial Regex CurpRegex();

    /// <summary>
    /// Validates a Mexican CURP.
    /// </summary>
    /// <param name="curp">The CURP string.</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateStatic(string? curp)
    {
        if (string.IsNullOrWhiteSpace(curp))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = curp.ToUpperInvariant().Trim();

        if (clean.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidCurpLength);
        }

        if (!CurpRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCurpFormat);
        }

        // Check Digit Calculation
        // Mapping: 0-9 -> 0-9, A-Z -> 10-35
        // Weights: 18, 17, ..., 2
        // Sum % 10
        // Check Digit = (10 - (Sum % 10)) % 10

        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            int val = GetCurpCharValue(clean[i]);
            sum += val * (18 - i);
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        int actualCheckDigit = clean[17] - '0';

        if (checkDigit != actualCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCurpChecksum);
        }

        return ValidationResult.Success();
    }

    private static int GetCurpCharValue(char c)
    {
        if (char.IsDigit(c))
        {
            return c - '0';
        }
        return c - 'A' + 10;
    }
}
