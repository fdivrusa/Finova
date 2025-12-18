using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Luxembourg.Validators;

public partial class LuxembourgVatValidator : IVatValidator, ITaxIdValidator
{
    private const string VatPrefix = "LU";

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => ValidateVat(instance);

    public ValidationResult Validate(string? instance) => ValidateVat(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    string? IValidator<string>.Parse(string? instance) => Normalize(instance);

    public static ValidationResult ValidateVat(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (cleaned.Length != 8 || !long.TryParse(cleaned, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidLuxembourgVatFormat);
        }

        // Checksum Validation (Mod 89)
        // First 6 digits % 89 == Last 2 digits
        int firstPart = int.Parse(cleaned[..6]);
        int checkDigits = int.Parse(cleaned[^2..]);

        if (firstPart % 89 != checkDigits)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidLuxembourgVatChecksum);
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (!ValidateVat(vat).IsValid)
        {
            return null;
        }

        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = VatPrefix,
            VatNumber = cleaned,
            IsValid = true
        };
    }

    public static string Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            return string.Empty;
        }

        var cleaned = number.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        return VatSanitizer.Sanitize(cleaned) ?? string.Empty;
    }
}
