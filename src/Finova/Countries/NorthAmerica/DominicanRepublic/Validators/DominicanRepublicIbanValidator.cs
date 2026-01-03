using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.DominicanRepublic.Validators;

/// <summary>
/// Validator for Dominican Republic IBANs.
/// Dominican Republic IBAN format: DO + 2 check digits + 4 letters (bank code) + 20 digits (account)
/// Length: 28 characters
/// Example: DO28BAGR00000001212453611324
/// </summary>
public class DominicanRepublicIbanValidator : IIbanValidator
{
    public string CountryCode => "DO";

    private const int DominicanRepublicIbanLength = 28;
    private const string DominicanRepublicCountryCode = "DO";

    public ValidationResult Validate(string? iban) => ValidateDominicanRepublicIban(iban);

    public static ValidationResult ValidateDominicanRepublicIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != DominicanRepublicIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, DominicanRepublicIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(DominicanRepublicCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 20 digits (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (20 digits)
        if (!bban.Substring(4).All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
