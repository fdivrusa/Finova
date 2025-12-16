using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

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

        // Structure Validation:
        // 1. Bank Code (Pos 4-7): 3 digits
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeNumeric);
            }
        }

        // 2. Account Number (Pos 7-17): 10 alphanumeric characters
        for (int i = 7; i < 17; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeAlphanumeric);
            }
        }

        // 3. National Check Digits (Pos 17-19): 2 digits
        for (int i = 17; i < 19; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.NationalCheckDigitsMustBeNumeric);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
