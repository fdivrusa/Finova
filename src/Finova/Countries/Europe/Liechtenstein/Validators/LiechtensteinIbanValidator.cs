using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Liechtenstein.Validators;

/// <summary>
/// Validator for Liechtenstein IBANs.
/// </summary>
public class LiechtensteinIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Liechtenstein.
    /// </summary>
    public string CountryCode => "LI";

    private const int LiechtensteinIbanLength = 21;
    private const string LiechtensteinCountryCode = "LI";

    /// <summary>
    /// Validates the Liechtenstein IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public ValidationResult Validate(string? iban) => ValidateLiechtensteinIban(iban);

    public static ValidationResult ValidateLiechtensteinIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LiechtensteinIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {LiechtensteinIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(LiechtensteinCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected LI.");
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-9): 5 digits
        for (int i = 4; i < 9; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bank code must be numeric.");
            }
        }

        // 2. Account Number (Pos 9-21): 12 alphanumeric characters
        for (int i = 9; i < 21; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Account number must be alphanumeric.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
}
