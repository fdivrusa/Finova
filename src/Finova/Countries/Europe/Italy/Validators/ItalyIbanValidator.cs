using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian bank accounts.
/// Italy IBAN format: IT + 2 check + 1 CIN + 5 ABI + 5 CAB + 12 Account (27 characters total).
/// </summary>
public class ItalyIbanValidator : IIbanValidator
{
    public string CountryCode => "IT";

    private const int ItalyIbanLength = 27;
    private const string ItalyCountryCode = "IT";

    #region Instance Methods (for Dependency Injection)

    public ValidationResult Validate(string? iban) => ValidateItalyIban(iban);

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates an Italian IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    public static ValidationResult ValidateItalyIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // 1. Check length (Italy IBAN is exactly 27 characters)
        if (normalized.Length != ItalyIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, ItalyIbanLength, normalized.Length));
        }

        // 2. Check country code
        if (!normalized.StartsWith(ItalyCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidItalyCountryCode);
        }

        // 3. Validate BBAN (CIN + ABI + CAB + Account)
        // BBAN is from index 4 to end (23 chars)
        string bban = normalized.Substring(4);
        var bbanResult = ItalyBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        // 4. Global Checksum (Modulo 97)
        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

    #endregion
}
