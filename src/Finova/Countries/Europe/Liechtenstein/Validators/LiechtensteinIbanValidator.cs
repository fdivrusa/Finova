using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != LiechtensteinIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, LiechtensteinIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(LiechtensteinCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidLiechtensteinCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = LiechtensteinBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
