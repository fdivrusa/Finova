using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian IBANs.
/// </summary>
public class AlbaniaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Albania.
    /// </summary>
    public string CountryCode => "AL";

    private const int AlbaniaIbanLength = 28;
    private const string AlbaniaCountryCode = "AL";

    /// <summary>
    /// Validates the Albanian IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    public ValidationResult Validate(string? iban) => ValidateAlbaniaIban(iban);

    public static ValidationResult ValidateAlbaniaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AlbaniaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, AlbaniaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(AlbaniaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "AL"));
        }

        // Structure Validation:
        // 1. Bank Code (Pos 4-7): 3 digits
        for (int i = 4; i < 7; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidBankCode);
            }
        }

        // 2. Branch Code (Pos 7-11): 4 digits
        for (int i = 7; i < 11; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidBranchCode);
            }
        }

        // 3. Control Character (Pos 11): 1 alphanumeric character (usually check digit for BBAN, but part of IBAN structure)
        if (!char.IsLetterOrDigit(normalized[11]))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidControlChar);
        }

        // 4. Account Number (Pos 12-28): 16 alphanumeric characters
        for (int i = 12; i < 28; i++)
        {
            if (!char.IsLetterOrDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.AlbaniaIbanInvalidAccount);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
