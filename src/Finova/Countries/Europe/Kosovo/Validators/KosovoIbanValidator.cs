using System.Diagnostics.CodeAnalysis;

using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;


namespace Finova.Countries.Europe.Kosovo.Validators;

/// <summary>
/// Validator for Kosovo IBANs.
/// </summary>
public class KosovoIbanValidator : IIbanValidator
{
    public string CountryCode => "XK";

    private const int KosovoIbanLength = 20;
    private const string KosovoCountryCode = "XK";

    public ValidationResult Validate(string? iban) => ValidateKosovoIban(iban);

    public static ValidationResult ValidateKosovoIban([NotNullWhen(true)] string? iban)
    {
        if (string.IsNullOrWhiteSpace(iban))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = IbanHelper.NormalizeIban(iban);

        if (normalized.Length != KosovoIbanLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, KosovoIbanLength, normalized.Length));
        }

        if (!normalized.StartsWith(KosovoCountryCode, StringComparison.OrdinalIgnoreCase))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidKosovoCountryCode);
        }

        // Structure check: All digits
        for (int i = 2; i < KosovoIbanLength; i++)
        {
            if (!char.IsDigit(normalized[i]))
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.KosovoIbanMustContainOnlyDigits);
            }
        }

        return IbanHelper.IsValidIban(normalized)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
    }
}

