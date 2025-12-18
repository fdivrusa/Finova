using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Vatican.Validators;

public partial class VaticanCityVatValidator : IVatValidator, ITaxIdValidator
{
    [GeneratedRegex(@"^\d{11}$")]
    private static partial Regex VatRegex();

    public string CountryCode => "VA";

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public ValidationResult Validate(string? number) => ValidateVat(number);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    string? IValidator<string>.Parse(string? instance) => Normalize(instance);

    public static ValidationResult ValidateVat(string? number)
    {
        number = VatSanitizer.Sanitize(number);

        if (string.IsNullOrWhiteSpace(number))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("VA"))
        {
            cleaned = cleaned[2..];
        }
        else if (cleaned.StartsWith("IT")) // Handle IT prefix as well since they use Italian IDs
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidVaticanVatFormat);
        }

        // Italian Partita IVA Logic (Luhn on 11 digits)
        if (!ChecksumHelper.ValidateLuhn(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidVaticanVatChecksum);
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = ValidateVat(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith("VA"))
        {
            cleaned = cleaned[2..];
        }
        else if (cleaned.StartsWith("IT"))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = "VA",
            VatNumber = cleaned,
            IsValid = true
        };
    }

    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace("VA", "").Replace("IT", "").Replace(" ", "");
        return VatRegex().IsMatch(cleaned) ? cleaned : null;
    }
}
