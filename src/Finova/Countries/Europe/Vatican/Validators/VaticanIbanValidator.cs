using System.Diagnostics.CodeAnalysis;
using Finova.Core.Iban;
using Finova.Core.Common;

namespace Finova.Countries.Europe.Vatican.Validators;

public class VaticanIbanValidator : IIbanValidator
{
    public string CountryCode => "VA";
    private const int VaticanIbanLength = 22;
    private const string VaticanCountryCode = "VA";

    public ValidationResult Validate(string? iban) => ValidateVaticanIban(iban);

    public static ValidationResult ValidateVaticanIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != VaticanIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidIbanLength, VaticanIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(VaticanCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidCountryCode);
        }

        // Structure check: All digits
        for (int i = 2; i < VaticanIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidVaticanIbanFormat);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}
