using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Spain.Validators;

/// <summary>
/// Validator for Spanish bank accounts.
/// Format: ES (2) + Check (2) + Entidad (4) + Oficina (4) + DC (2) + Cuenta (10).
/// Total Length: 24.
/// The 'DC' (Dígito de Control) validates the Bank/Branch and the Account separately using Modulo 11.
/// </summary>
public class SpainIbanValidator : IIbanValidator
{
    public string CountryCode => "ES";
    private const int SpainIbanLength = 24;
    private const string SpainCountryCode = "ES";

    // Standard weights for Spanish Modulo 11 algorithm

    public ValidationResult Validate(string? iban) => ValidateSpainIban(iban);

    public static ValidationResult ValidateSpainIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != SpainIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, SpainIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(SpainCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "ES"));
        }

        // Validate BBAN (Bank + Branch + DC + Account)
        // BBAN starts at index 4
        string bban = normalized.Substring(4);
        var bbanResult = SpainBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }

}
