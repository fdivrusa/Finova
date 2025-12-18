using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Portugal.Validators;

public class PortugalIbanValidator : IIbanValidator
{
    public string CountryCode => "PT";
    private const int PortugalIbanLength = 25;

    public ValidationResult Validate(string? iban) => ValidatePortugalIban(iban);

    public static ValidationResult ValidatePortugalIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }
        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != PortugalIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, PortugalIbanLength, normalized.Length));
        }


        if (!normalized.StartsWith("PT", StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, string.Format(ValidationMessages.InvalidCountryCodeExpected, "PT"));
        }

        // Extract the parts relevant to NIB (Positions 4 to 25)
        // BBAN is 21 digits
        string bban = normalized.Substring(4, 21);

        var bbanResult = PortugalBbanValidator.Validate(bban);
        if (!bbanResult.IsValid)
        {
            return bbanResult;
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
