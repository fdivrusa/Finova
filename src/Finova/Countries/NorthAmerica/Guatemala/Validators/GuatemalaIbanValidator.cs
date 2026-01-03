using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.Guatemala.Validators;

/// <summary>
/// Validator for Guatemala IBANs.
/// Guatemala IBAN format: GT + 2 check digits + 4 letters (bank code) + 20 alphanumeric (account)
/// Length: 28 characters
/// Example: GT82TRAJ01020000001210029690
/// </summary>
public class GuatemalaIbanValidator : IIbanValidator
{
    public string CountryCode => "GT";

    private const int GuatemalaIbanLength = 28;
    private const string GuatemalaCountryCode = "GT";

    public ValidationResult Validate(string? iban) => ValidateGuatemalaIban(iban);

    public static ValidationResult ValidateGuatemalaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != GuatemalaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, GuatemalaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(GuatemalaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Validate BBAN format: 4 letters (bank code) + 20 alphanumeric (account)
        string bban = normalized.Substring(4);

        // Bank code (4 letters)
        if (!bban.Substring(0, 4).All(char.IsLetter))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        // Account number (20 alphanumeric)
        if (!bban.Substring(4).All(char.IsLetterOrDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidFormat);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
