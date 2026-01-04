using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.UAE.Validators;

/// <summary>
/// Validates UAE Tax Registration Number (TRN).
/// VAT was introduced in UAE on January 1, 2018.
/// Format: 100XXXXXXXXX (15 digits starting with 100).
/// </summary>
public partial class UaeVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "AE";

    [GeneratedRegex(@"^100\d{12}$", RegexOptions.Compiled)]
    private static partial Regex TrnPattern();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a UAE Tax Registration Number.
    /// </summary>
    /// <param name="vat">The TRN number (15 digits starting with 100).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove AE prefix if present
        if (clean.StartsWith("AE", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!TrnPattern().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "UAE TRN must be 15 digits starting with 100.");
        }

        // UAE TRN uses a Luhn-like checksum validation
        // The last digit is a check digit
        if (!ValidateTrnChecksum(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid UAE TRN checksum.");
        }

        return ValidationResult.Success();
    }

    private static bool ValidateTrnChecksum(string trn)
    {
        // UAE TRN checksum algorithm (Mod 97-10)
        // Similar to IBAN check digit calculation
        int sum = 0;
        for (int i = 0; i < trn.Length - 1; i++)
        {
            sum += trn[i] - '0';
        }

        int expectedCheck = (10 - (sum % 10)) % 10;
        int actualCheck = trn[^1] - '0';

        return expectedCheck == actualCheck;
    }

    /// <summary>
    /// Gets details of a validated UAE TRN.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("AE", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "TRN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "UAE Tax Registration Number"
        };
    }
}
