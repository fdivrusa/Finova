using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Andorra.Validators;

/// <summary>
/// Validator for Andorra VAT numbers (NRT - Número de Registre Tributari).
/// Format: [A-Z][0-9]{6}[A-Z] (8 characters).
/// Example: U123456A
/// </summary>
public partial class AndorraVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "AD";
    private const int VatLength = 8;

    [GeneratedRegex(@"^[A-Z]\d{6}[A-Z]$")]
    private static partial Regex AndorraVatRegex();

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

        // Remove AD prefix if present
        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }

        if (normalized.Length != VatLength)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected {VatLength} characters.");
        }

        if (!AndorraVatRegex().IsMatch(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid format. Expected Letter + 6 digits + Letter.");
        }

        // No public checksum algorithm available.
        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

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

        return new VatDetails
        {
            VatNumber = $"{CountryCodePrefix}{normalized}",
            CountryCode = CountryCodePrefix,
            IsValid = true
        };
    }
}
