using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;



namespace Finova.Countries.Europe.Georgia.Validators;

/// <summary>
/// Validator for Georgia IBANs.
/// </summary>
public class GeorgiaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Georgia.
    /// </summary>
    public string CountryCode => "GE";

    /// <summary>
    /// Validates the Georgia IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateGeorgiaIban(iban);

    /// <summary>
    /// Static validation method for Georgia IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateGeorgiaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 22)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected 22, got {normalized.Length}.");
        }

        if (!normalized.StartsWith("GE", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected GE.");
        }

        // GE + 2 alphanumeric (Bank) + 16 digits (Account)
        // Indices 4-5 must be alphanumeric
        for (int i = 4; i < 6; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bank Code must be alphanumeric.");
            }
        }

        // Indices 6-21 must be alphanumeric
        for (int i = 6; i < 22; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Account Number must be alphanumeric.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}

