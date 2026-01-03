using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;

namespace Finova.Countries.NorthAmerica.CostaRica.Validators;

/// <summary>
/// Validator for Costa Rica IBANs.
/// Costa Rica IBAN format: CR + 2 check digits + 1 digit (reserve) + 3 digits (bank code) + 14 digits (account)
/// Length: 22 characters
/// Example: CR05015202001026284066
/// </summary>
public class CostaRicaIbanValidator : IIbanValidator
{
    public string CountryCode => "CR";

    private const int CostaRicaIbanLength = 22;
    private const string CostaRicaCountryCode = "CR";

    public ValidationResult Validate(string? iban) => ValidateCostaRicaIban(iban);

    public static ValidationResult ValidateCostaRicaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != CostaRicaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength,
                string.Format(ValidationMessages.InvalidLengthExpectedXGotY, CostaRicaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(CostaRicaCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // BBAN (18 digits: 1 reserve + 3 bank + 14 account)
        string bban = normalized.Substring(4);
        if (!bban.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
