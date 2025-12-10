using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Ireland.Validators;

/// <summary>
/// Validator for Irish bank accounts.
/// Ireland IBAN format: IE + 2 check + 4 Bank Code + 6 Sort Code + 8 Account (22 characters total).
/// </summary>
public class IrelandIbanValidator : IIbanValidator
{
    public string CountryCode => "IE";

    private const int IrelandIbanLength = 22;
    private const string IrelandCountryCode = "IE";

    #region Instance Methods (for Dependency Injection)

    public ValidationResult Validate(string? iban) => ValidateIrelandIban(iban);

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates an Irish IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    public static ValidationResult ValidateIrelandIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "IBAN cannot be empty.");
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Check length (Ireland IBAN is exactly 22 characters)
        if (normalized.Length != IrelandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {IrelandIbanLength}, got {normalized.Length}.");
        }

        // Check country code
        if (!normalized.StartsWith(IrelandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid country code. Expected IE.");
        }

        // Structure Validation:

        // Bank Code / BIC (Pos 4-7) must be letters
        // Example: "AIBK"
        for (int i = 4; i < 8; i++)
        {
            if (!char.IsLetter(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Ireland Bank Code must be letters.");
            }
        }

        // Sort Code (Pos 8-13) and Account Number (Pos 14-21) must be digits
        // Total range from index 8 to 21
        for (int i = 8; i < 22; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Ireland Sort Code/Account Number must be digits.");
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
    }

    #endregion
}
