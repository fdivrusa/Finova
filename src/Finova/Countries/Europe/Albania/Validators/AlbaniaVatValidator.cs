using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Albania.Validators;

/// <summary>
/// Validator for Albanian VAT numbers (NIPT).
/// Format: 10 chars. [A-Z] + 8 digits + [A-Z].
/// Note: Checksum algorithm is not publicly standardized enough for safe validation.
/// We rely on strict Regex validation only.
/// </summary>
public partial class AlbaniaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "AL";
    private const int VatLength = 10;

    // Strict Regex: 1 Letter + 8 Digits + 1 Letter
    [GeneratedRegex(@"^[A-Z]\d{8}[A-Z]$")]
    private static partial Regex AlbaniaVatRegex();

    public string CountryCode => CountryCodePrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var normalized = vat.Trim().ToUpperInvariant();

        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }

        if (normalized.Length != VatLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidLengthExpectedX, VatLength));
        }

        // Format Check (Sufficient for Albania)
        if (!AlbaniaVatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, string.Format(ValidationMessages.InvalidVatFormat, "Albania"));
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var normalized = vat!.Trim().ToUpperInvariant();
        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }

        return new VatDetails
        {
            VatNumber = $"{CountryCodePrefix}{normalized}",
            CountryCode = CountryCodePrefix,
            IsValid = true
        };
    }
}
