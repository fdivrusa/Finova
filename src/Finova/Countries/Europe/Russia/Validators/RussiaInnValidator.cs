using Finova.Core.Common;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Russia.Validators;

/// <summary>
/// Validator for Russian Tax Identification Number (INN).
/// INN (Identifikatsionny Nomer Nalogoplatelshchika) is 10 digits for corporations and 12 for individuals.
/// </summary>
public class RussiaInnValidator : ITaxIdValidator, IVatValidator
{
    public string CountryCode => "RU";

    public ValidationResult Validate(string? input) => ValidateInn(input);

    /// <summary>
    /// Explicit implementation for IVatValidator.
    /// </summary>
    VatDetails? IValidator<VatDetails>.Parse(string? input)
    {
        var result = Validate(input);
        if (!result.IsValid)
        {
            return null;
        }

        return new VatDetails
        {
            CountryCode = "RU",
            VatNumber = input!.Trim(),
            IsValid = true,
            IdentifierKind = "INN"
        };
    }

    /// <summary>
    /// Implementation for ITaxIdValidator / IValidator&lt;string&gt;.
    /// </summary>
    public string? Parse(string? input) => Validate(input).IsValid ? input?.Trim().ToUpperInvariant() : null;

    public static ValidationResult ValidateInn(string? inn)
    {
        if (string.IsNullOrWhiteSpace(inn))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = inn.Trim();

        if (!clean.All(char.IsDigit))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.MustContainOnlyDigits);
        }

        if (clean.Length == 10)
        {
            return Validate10DigitInn(clean);
        }

        if (clean.Length == 12)
        {
            return Validate12DigitInn(clean);
        }

        return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "INN must be 10 or 12 digits.");
    }

    private static ValidationResult Validate10DigitInn(string inn)
    {
        int[] weights = { 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        int checksum = 0;
        for (int i = 0; i < 9; i++)
        {
            checksum += (inn[i] - '0') * weights[i];
        }
        int checkDigit = (checksum % 11) % 10;

        if (checkDigit != (inn[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    private static ValidationResult Validate12DigitInn(string inn)
    {
        int[] weights1 = { 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        int checksum1 = 0;
        for (int i = 0; i < 10; i++)
        {
            checksum1 += (inn[i] - '0') * weights1[i];
        }
        int checkDigit1 = (checksum1 % 11) % 10;

        if (checkDigit1 != (inn[10] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        int[] weights2 = { 3, 7, 2, 4, 10, 3, 5, 9, 4, 6, 8 };
        int checksum2 = 0;
        for (int i = 0; i < 11; i++)
        {
            checksum2 += (inn[i] - '0') * weights2[i];
        }
        int checkDigit2 = (checksum2 % 11) % 10;

        if (checkDigit2 != (inn[11] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }
}