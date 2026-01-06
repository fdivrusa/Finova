using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.SouthKorea.Validators;

/// <summary>
/// Validates South Korean Business Registration Number (BRN / 사업자등록번호).
/// This is used for VAT registration in South Korea.
/// Format: XXX-XX-XXXXX (10 digits with dashes) or XXXXXXXXXX (10 digits).
/// </summary>
public partial class SouthKoreaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "KR";

    [GeneratedRegex(@"^\d{10}$", RegexOptions.Compiled)]
    private static partial Regex BrnPattern();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a South Korean Business Registration Number.
    /// </summary>
    /// <param name="vat">The BRN number (10 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove KR prefix if present
        if (clean.StartsWith("KR", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!BrnPattern().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSouthKoreaVatFormat);
        }

        // Validate checksum
        // Weights: 1, 3, 7, 1, 3, 7, 1, 3, 5
        int[] weights = [1, 3, 7, 1, 3, 7, 1, 3, 5];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        // Add additional digit from position 8
        sum += ((clean[8] - '0') * 5) / 10;

        int checkDigit = (10 - (sum % 10)) % 10;

        if (checkDigit != (clean[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSouthKoreaVatChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Gets details of a validated South Korean BRN.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("KR", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "BRN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "South Korean Business Registration Number (사업자등록번호)"
        };
    }
}
