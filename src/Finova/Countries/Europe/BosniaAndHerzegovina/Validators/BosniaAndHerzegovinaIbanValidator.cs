using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;
using System.Text.RegularExpressions;



namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

/// <summary>
/// Validator for Bosnia and Herzegovina IBANs.
/// </summary>
public class BosniaAndHerzegovinaIbanValidator : IIbanValidator
{
    /// <summary>
    /// Gets the country code for Bosnia and Herzegovina.
    /// </summary>
    public string CountryCode => "BA";

    /// <summary>
    /// Validates the Bosnia and Herzegovina IBAN.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public ValidationResult Validate(string? iban) => ValidateBosniaAndHerzegovinaIban(iban);

    /// <summary>
    /// Static validation method for Bosnia and Herzegovina IBANs.
    /// </summary>
    /// <param name="iban">The IBAN to validate.</param>
    /// <returns>ValidationResult indicating success or failure.</returns>
    public static ValidationResult ValidateBosniaAndHerzegovinaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLengthExpected, 20, normalized.Length));
        }

        if (!normalized.StartsWith("BA", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "BA"));
        }

        // BA + 18 digits
        // Indices 4-19 must be digits
        for (int i = 4; i < 20; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidIbanDigitsOnly, "Bosnia and Herzegovina"));
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

