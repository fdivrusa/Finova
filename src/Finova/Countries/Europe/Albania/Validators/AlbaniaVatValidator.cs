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

    // Regex stricte : 1 Lettre + 8 Chiffres + 1 Lettre
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        var normalized = vat.Trim().ToUpperInvariant();

        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }

        if (normalized.Length != VatLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {VatLength} characters.");
        }

        // Format Check (Suffisant pour l'Albanie)
        if (!AlbaniaVatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format. Expected Letter + 8 digits + Letter.");
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