using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Austria.Validators;

public class AustriaIbanValidator : IIbanValidator
{
    public string CountryCode => "AT";
    private const int AustriaIbanLength = 20;

    public ValidationResult Validate(string? iban) => ValidateAustriaIban(iban);

    public static ValidationResult ValidateAustriaIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.IbanEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != AustriaIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, AustriaIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "AT"));
        }

        // Validate BBAN
        string bban = normalized.Substring(4);
        var bbanResult = AustriaBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
