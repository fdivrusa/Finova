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

        // Structure Validation:
        // 1. Bank Code (Pos 4-7): 3 digits
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.BankCodeMustBeNumeric);
            }
        }

        // 2. Account Number (Pos 7-20): 13 digits
        for (int i = 7; i < 20; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AccountNumberMustBeNumeric);
            }
        }

        // 3. National Check Digits (Pos 20-22): 2 digits
        for (int i = 20; i < 22; i++)
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
