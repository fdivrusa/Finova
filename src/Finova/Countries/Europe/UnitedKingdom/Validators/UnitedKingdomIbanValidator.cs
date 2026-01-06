using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.UnitedKingdom.Validators;

/// <summary>
/// Validator for United Kingdom bank accounts.
/// United Kingdom IBAN format: GB + 2 check digits + 4 bank code + 6 sort code + 8 account number (22 characters total).
/// Example: GB29NWBK60161331926819 or formatted: GB29 NWBK 6016 1331 9268 19
/// </summary>
public class UnitedKingdomIbanValidator : IIbanValidator
{
    public string CountryCode => "GB";

    private const int UnitedKingdomIbanLength = 22;
    private const string UnitedKingdomCountryCode = "GB";

    #region Instance Methods (for Dependency Injection)
    public ValidationResult Validate([NotNullWhen(true)] string? iban) => ValidateUnitedKingdomIban(iban);
    #endregion

    #region Static Methods (for Direct Usage)
    /// <summary>
    /// Validates a United Kingdom IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns> True if the IBAN is valid for United Kingdom; otherwise, false.</returns>
    public static ValidationResult ValidateUnitedKingdomIban(string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);
        // Check length (United Kingdom IBAN is exactly 22 characters)
        if (normalized.Length != UnitedKingdomIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, UnitedKingdomIbanLength, normalized.Length));
        }

        // Check country code
        if (!normalized.StartsWith(UnitedKingdomCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, UnitedKingdomCountryCode));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = UnitedKingdomBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        // Validate structure and checksum
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
    #endregion
}
