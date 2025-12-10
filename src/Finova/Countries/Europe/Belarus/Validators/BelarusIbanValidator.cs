using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.Belarus.Validators;

/// <summary>
/// Validator for Belarus IBANs.
/// </summary>
public class BelarusIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Belarus.
    /// </summary>
    public string CountryCode => "BY";

    /// <summary>
    /// Validates the Belarus IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateBelarusIban(iban);

    /// <summary>
    /// Static validation method for Belarus IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public static ValidationResult ValidateBelarusIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 28)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected 28, got {normalized.Length}.");
        }

        if (!normalized.StartsWith("BY", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected BY.");
        }

        // BY + 4 alphanumeric + 22 alphanumeric
        // Indices 4-27 must be alphanumeric
        for (int i = 4; i < 28; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Belarus IBAN must contain only alphanumeric characters after the country code.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}

