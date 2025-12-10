using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.Greenland.Validators;

/// <summary>
/// Validator for Greenland IBANs.
/// </summary>
public class GreenlandIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Greenland.
    /// </summary>
    public string CountryCode => "GL";

    /// <summary>
    /// Validates the Greenland IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateGreenlandIban(iban);

    /// <summary>
    /// Static validation method for Greenland IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateGreenlandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 18)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected 18, got {normalized.Length}.");
        }

        if (!normalized.StartsWith("GL", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected GL.");
        }

        // GL + 16 digits
        // Indices 4-17 must be digits
        for (int i = 4; i < 18; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Greenland IBAN must contain only digits after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}

