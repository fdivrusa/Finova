using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Montenegro.Validators;

/// <summary>
/// Validator for Montenegro IBANs.
/// </summary>
public class MontenegroIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Montenegro.
    /// </summary>
    public string CountryCode => "ME";

    private const int MontenegroIbanLength = 22;
    private const string MontenegroCountryCode = "ME";

    /// <summary>
    /// Validates the Montenegro IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public ValidationResult Validate(string? iban) => ValidateMontenegroIban(iban);

    public static ValidationResult ValidateMontenegroIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != MontenegroIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, MontenegroIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(MontenegroCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidMontenegroCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = MontenegroBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
