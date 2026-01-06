using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MoldovaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, MoldovaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(MoldovaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidMoldovaCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = MoldovaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
