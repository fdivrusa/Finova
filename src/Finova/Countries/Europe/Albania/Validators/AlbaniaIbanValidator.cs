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

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = AlbaniaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
