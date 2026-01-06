using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.Europe.NorthMacedonia.Validators;

/// <summary>
/// Validator for North Macedonia IBANs.
/// </summary>
public class NorthMacedoniaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for North Macedonia.
    /// </summary>
    public string CountryCode => "MK";

    private const int NorthMacedoniaIbanLength = 19;
    private const string NorthMacedoniaCountryCode = "MK";

    /// <summary>
    /// Validates the North Macedonia IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public ValidationResult Validate(string? iban) => ValidateNorthMacedoniaIban(iban);

    public static ValidationResult ValidateNorthMacedoniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != NorthMacedoniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, NorthMacedoniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(NorthMacedoniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidNorthMacedoniaCountryCode);
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = NorthMacedoniaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
