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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        // Check length (Ireland IBAN is exactly 22 characters)
        if (normalized.Length != IrelandIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, IrelandIbanLength, normalized.Length));
        }

        // Check country code
        if (!normalized.StartsWith(IrelandCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidIrelandCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = IrelandBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
    #endregion
}
