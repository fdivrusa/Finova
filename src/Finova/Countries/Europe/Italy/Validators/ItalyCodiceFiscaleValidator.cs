using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian Codice Fiscale (Tax Code) and Partita IVA (VAT Number).
/// </summary>
public partial class ItalyCodiceFiscaleValidator : ITaxIdValidator
{
    [GeneratedRegex(@"^[A-Z]{6}\d{2}[A-Z]\d{2}[A-Z]\d{3}[A-Z]$")]
    private static partial Regex CodiceFiscaleRegex();

    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex PartitaIvaRegex();

    [GeneratedRegex(@"[^\w]")]
    private static partial Regex AlphanumericOnlyRegex();

    public string CountryCode => "IT";

    public ValidationResult Validate(string? instance) => ValidateItalianIdentifier(instance);

    public string? Parse(string? instance) => Normalize(instance);

    /// <summary>
    /// Validates an Italian Codice Fiscale or Partita IVA.
    /// </summary>
    public static ValidationResult ValidateItalianIdentifier(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = Normalize(number);

        if (normalized.Length == 16)
        {
            return ItalyNationalIdValidator.ValidateStatic(normalized);
        }
        else if (normalized.Length == 11)
        {
            return ValidatePartitaIva(normalized);
        }
        else
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidItalianIdentifierLength);
        }
    }

    private static ValidationResult ValidateCodiceFiscale(string cf)
    {
        return ItalyNationalIdValidator.ValidateStatic(cf);
    }

    private static ValidationResult ValidatePartitaIva(string piva)
    {
        if (!PartitaIvaRegex().IsMatch(piva))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidPartitaIvaFormat);
        }

        // Luhn Algorithm (standard for Partita IVA)
        return ChecksumHelper.ValidateLuhn(piva)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidCheckDigit, ValidationMessages.InvalidPartitaIvaCheckDigit);
    }

    public static string Format(string? number)
    {
        if (!ValidateItalianIdentifier(number).IsValid)
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
