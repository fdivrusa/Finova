using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Moldova.Validators;

/// <summary>
/// Validator for Moldovan IBANs.
/// </summary>
public class MoldovaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Moldova.
    /// </summary>
    public string CountryCode => "MD";

    private const int MoldovaIbanLength = 24;
    private const string MoldovaCountryCode = "MD";

    /// <summary>
    /// Validates the Moldovan IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public ValidationResult Validate(string? iban) => ValidateMoldovaIban(iban);

    public static ValidationResult ValidateMoldovaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MoldovaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {MoldovaIbanLength}, got {normalized.Length}.");
        }

        if (!normalized.StartsWith(MoldovaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected MD.");
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-6): 2 alphanumeric characters
        for (int i = 4; i < 6; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Bank code must be alphanumeric.");
            }
        }

        // 2. Account Number (Pos 6-24): 18 alphanumeric characters
        for (int i = 6; i < 24; i++)
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
