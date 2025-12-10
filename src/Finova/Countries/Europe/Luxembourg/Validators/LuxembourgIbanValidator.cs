using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Luxembourg.Validators;

/// <summary>
/// Validator for Luxembourg IBAN bank accounts.
/// Luxembourg IBAN format: LU + 2 check digits + 3 bank code + 13 account number (20 characters total).
/// Example: LU280019400644750000 or formatted: LU28 0019 4006 4475 0000
/// </summary>
public class LuxembourgIbanValidator : IIbanValidator
{
    public string CountryCode => "LU";

    private const int LuxembourgIbanLength = 20;
    private const string LuxembourgCountryCode = "LU";

    #region Instance Methods (for Dependency Injection)

    public ValidationResult Validate(string? iban) => ValidateLuxembourgIban(iban);

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates a Luxembourg IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate</param>
    /// <returns>True if valid Luxembourg IBAN, false otherwise</returns>
    public static ValidationResult ValidateLuxembourgIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        // Check length (Luxembourg IBAN is exactly 20 characters)
        if (normalized.Length != LuxembourgIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {LuxembourgIbanLength}, got {normalized.Length}.");
        }

        // Check country code
        if (!normalized.StartsWith(LuxembourgCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected LU.");
        }

        // Bank Code (indices 4-6) must be numeric.
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Luxembourg Bank Code must be numeric.");
            }
        }

        // Validate structure and checksum
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }
    #endregion
}
