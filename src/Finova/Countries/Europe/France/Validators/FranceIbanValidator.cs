using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for France bank accounts.
/// France IBAN format: FR + 2 check digits + 5 bank code + 5 branch code + 11 account number + 2 RIB key.
/// </summary>
public class FranceIbanValidator : IIbanValidator
{
    public string CountryCode => "FR";

    private const int FranceIbanLength = 27;
    private const string FranceCountryCode = "FR";

    public ValidationResult Validate(string? iban) => ValidateFranceIban(iban);

    public static ValidationResult ValidateFranceIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != FranceIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, FranceIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(FranceCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidFranceCountryCode);
        }

        // Validate BBAN (Bank + Branch + Account + Key)
        // BBAN starts at index 4
        string bban = normalized.Substring(4);
        var bbanResult = FranceBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

}
