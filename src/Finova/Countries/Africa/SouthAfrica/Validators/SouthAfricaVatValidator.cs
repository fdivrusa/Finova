using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Africa.SouthAfrica.Validators;

/// <summary>
/// Validates South African Value Added Tax (VAT) registration number.
/// Format: 10 digits, typically formatted as 4XXXXXXXXX
/// First digit is usually 4 for VAT vendors.
/// </summary>
public partial class SouthAfricaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "ZA";

    [GeneratedRegex(@"^4\d{9}$")]
    private static partial Regex VatRegex();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <summary>
    /// Cleans and normalizes a VAT number by removing whitespace, hyphens, and the ZA prefix.
    /// </summary>
    /// <param name="vat">The VAT number to clean.</param>
    /// <returns>The cleaned VAT number.</returns>
    private static string CleanVatNumber(string vat)
    {
        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove ZA prefix if present
        if (clean.StartsWith("ZA", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return clean;
    }

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a South African VAT number.
    /// </summary>
    /// <param name="vat">The VAT number string (10 digits starting with 4).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = CleanVatNumber(vat);

        if (clean.Length != 10)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, ValidationMessages.InvalidSouthAfricaVatLength);
        }

        if (!VatRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSouthAfricaVatFormat);
        }

        // Validate using Luhn algorithm
        if (!ChecksumHelper.ValidateLuhn(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Gets details of a validated South African VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = CleanVatNumber(vat!);

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "VAT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "South African Value Added Tax registration number"
        };
    }
}
