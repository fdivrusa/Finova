using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Norway.Validators;

public class NorwayIbanValidator : IIbanValidator
{
    public string CountryCode => "NO";
    private const int NorwayIbanLength = 15;
    private const string NorwayCountryCode = "NO";

    public ValidationResult Validate(string? iban) => ValidateNorwayIban(iban);

    public static ValidationResult ValidateNorwayIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != NorwayIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, NorwayIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(NorwayCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Internal Validation: Modulo 11 on BBAN (Last 11 digits)
        // Indices 4 to 15 in IBAN
        string bban = normalized.Substring(4, 11);
        var bbanResult = NorwayBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
