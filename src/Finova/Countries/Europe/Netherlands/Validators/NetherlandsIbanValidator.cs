using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.Netherlands.Validators;

/// <summary>
/// Validator for Dutch (Netherlands) IBAN bank accounts.
/// Dutch IBAN format: NL + 2 check digits + 4 bank code + 10 account number (18 characters total).
/// Example: NL91ABNA0417164300.
/// </summary>
public class NetherlandsIbanValidator : IIbanValidator
{
    public string CountryCode => "NL";

    private const int DutchIbanLength = 18;
    private const string DutchCountryCode = "NL";

    #region Instance Methods (for Dependency Injection)

    public ValidationResult Validate(string? iban) => ValidateNetherlandsIban(iban);

    #endregion

    #region Static Methods (for Direct Usage)

    /// <summary>
    /// Validates a Dutch IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate</param>
    /// <returns>True if valid Dutch IBAN, false otherwise</returns>
    public static ValidationResult ValidateNetherlandsIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != DutchIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, DutchIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(DutchCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidNetherlandsCountryCode);
        }

        // Bank Code: Positions 4-7 (4 chars) must be letters (e.g., ABNA, INGB, RABO)
        // Account Number: Positions 8-17 (10 chars) must be digits
        // BBAN is 14 chars (from index 4)
        string bban = normalized.Substring(4, 14);

        var bbanResult = NetherlandsBbanValidator.Validate(bban);
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
