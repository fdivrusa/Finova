using Finova.Core.Common;
using Finova.Core.Vat;
using Finova.Countries.Europe.France.Models;

namespace Finova.Countries.Europe.France.Validators;

public class FranceVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "FR";

    public string CountryCode => CountryCodePrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = vat.Trim().ToUpperInvariant();

        if (!normalized.StartsWith(CountryCodePrefix))
        {
            if (normalized.Length == 11 && long.TryParse(normalized, out _))
            {
                // Proceed
            }
            else
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, ValidationMessages.InvalidFranceVatFormatPrefixOrDigits);
            }
        }
        else
        {
            normalized = normalized[2..];
        }

        normalized = normalized.Replace(" ", "").Replace(".", "");

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedXGotY, 11, normalized.Length));
        }

        if (!long.TryParse(normalized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.FranceVatDigitsAfterPrefix);
        }

        var keyStr = normalized.Substring(0, 2);
        var sirenStr = normalized.Substring(2, 9);

        if (!int.TryParse(keyStr, out int key))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidNumericFormat);
        }

        int sirenMod97 = ChecksumHelper.CalculateModulo97(sirenStr);
        if (sirenMod97 == -1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSirenFormat);
        }

        int calculatedKey = (12 + 3 * sirenMod97) % 97;

        if (key != calculatedKey)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksumKey);
        }

        if (!ChecksumHelper.ValidateLuhn(sirenStr))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksumSirenLuhn);
        }

        return ValidationResult.Success();
    }

    public static FranceVatDetails? GetVatDetails(string? vat)
    {
        var result = Validate(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var normalized = vat!.Trim().ToUpperInvariant();
        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }
        normalized = normalized.Replace(" ", "").Replace(".", "");

        var siren = normalized.Substring(2, 9);

        return new FranceVatDetails
        {
            VatNumber = $"{CountryCodePrefix}{normalized}",
            CountryCode = CountryCodePrefix,
            IsValid = true,
            Siren = siren
        };
    }
}
