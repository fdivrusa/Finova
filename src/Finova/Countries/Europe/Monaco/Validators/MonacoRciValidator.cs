using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Monaco.Validators;

/// <summary>
/// Validator for Monaco RCI (Répertoire du Commerce et de l'Industrie).
/// Format: XX S XXXXX (Year + Sequence type + Number).
/// </summary>
public partial class MonacoRciValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{2}[SP]\d{5}$")]
    private static partial Regex RciRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex AlphaNumericRegex();

    public string CountryCode => "MC";

    public ValidationResult Validate(string? instance) => ValidateRci(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates a Monaco RCI.
    /// </summary>
    /// <param name="number">The number to validate.</param>
    /// <returns>Validation result.</returns>
    public static ValidationResult ValidateRci(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Remove spaces
        var cleaned = number.Trim().ToUpperInvariant();
        // Remove spaces explicitly as per "Sanitization: Remove spaces" (implied by regex pattern matching strictness?)
        // Spec says "Pattern XX S XXXXX". Regex `^\d{2}[SP]\d{5}$`.
        // So we should remove spaces to match regex.
        cleaned = cleaned.Replace(" ", "");

        if (!RciRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMonacoRciFormat);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Formats a Monaco RCI.
    /// </summary>
    public static string Format(string? number)
    {
        if (!ValidateRci(number).IsValid)
        {
            throw new ArgumentException("Invalid RCI", nameof(number));
        }

        var normalized = Normalize(number);
        // Format: XX S XXXXX
        return $"{normalized.Substring(0, 2)} {normalized.Substring(2, 1)} {normalized.Substring(3, 5)}";
    }

    /// <summary>
    /// Normalizes a Monaco RCI.
    /// </summary>
    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        return cleaned.Replace(" ", "");
    }
}
