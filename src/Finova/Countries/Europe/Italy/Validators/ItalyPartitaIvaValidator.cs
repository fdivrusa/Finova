using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian Partita IVA (VAT Number).
/// </summary>
public partial class ItalyPartitaIvaValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex PartitaIvaRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex AlphanumericOnlyRegex();

    public string CountryCode => "IT";

    public ValidationResult Validate(string? instance) => ValidatePartitaIvaStatic(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates an Italian Partita IVA.
    /// </summary>
    public static ValidationResult ValidatePartitaIvaStatic(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(number);

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidLength);
        }

        if (!PartitaIvaRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidPartitaIvaFormat);
        }

        // Luhn Algorithm (standard for Partita IVA)
        return ChecksumHelper.ValidateLuhn(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidPartitaIvaCheckDigit);
    }

    public static string Format(string? number)
    {
        if (!ValidatePartitaIvaStatic(number).IsValid)
        {
            throw new ArgumentException("Invalid Input", nameof(number));
        }
        return Normalize(number);
    }

    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        return AlphanumericOnlyRegex().Replace(number, "").ToUpperInvariant();
    }
}
