using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.SanMarino.Validators;

public partial class SanMarinoVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{5}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SM";

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Empty.");

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix)) cleaned = cleaned[2..];

        if (!VatRegex().IsMatch(cleaned))
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid San Marino VAT format.");

        // No public checksum available.
        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = Validate(vat);
        if (!result.IsValid) return null;

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix)) cleaned = cleaned[2..];

        return new VatDetails { CountryCode = VatPrefix, VatNumber = cleaned, IsValid = true };
    }
}