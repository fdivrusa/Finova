using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Turkey.Validators;

public partial class TurkeyVknValidator : IVatValidator, ITaxIdValidator
{
    [GeneratedRegex(@"^\d{10}$")]
    private static partial Regex VknRegex();

    private const string CountryCodeVal = "TR";

    public string CountryCode => CountryCodeVal;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public ValidationResult Validate(string? number) => ValidateVkn(number);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    string? IValidator<string>.Parse(string? instance) => Normalize(instance);

    public static ValidationResult ValidateVkn(string? number)
    {
        number = VatSanitizer.Sanitize(number);

        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(CountryCodeVal))
        {
            cleaned = cleaned[2..];
        }

        if (!VknRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidTurkeyVknFormat);
        }

        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = cleaned[i] - '0';
            int tmp = (digit + (10 - (i + 1))) % 10;
            int val;

            if (tmp == 9)
            {
                val = 9;
            }
            else
            {
                // (tmp * 2^(10-(i+1))) % 9
                // Use bit shifting for power of 2: 1 << (10 - (i + 1))
                val = (tmp * (1 << (10 - (i + 1)))) % 9;
            }

            sum += val;
        }

        int checkDigit = (10 - (sum % 10)) % 10;
        int lastDigit = cleaned[9] - '0';

        if (lastDigit != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidTurkeyVknChecksum);
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = ValidateVkn(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(CountryCodeVal))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = CountryCodeVal,
            VatNumber = cleaned,
            IsValid = true
        };
    }

    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace(CountryCodeVal, "").Replace(" ", "");
        return VknRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
