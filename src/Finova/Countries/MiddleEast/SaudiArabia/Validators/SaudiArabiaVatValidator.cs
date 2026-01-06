using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.SaudiArabia.Validators;

/// <summary>
/// Validates Saudi Arabia VAT Registration Number.
/// VAT was introduced in Saudi Arabia on January 1, 2018.
/// Format: 3XXXXXXXXXXXXXX (15 digits starting with 3).
/// The first digit (3) indicates Saudi Arabia in the GCC region.
/// </summary>
public partial class SaudiArabiaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "SA";

    [GeneratedRegex(@"^3\d{14}$", RegexOptions.Compiled)]
    private static partial Regex VatPattern();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Saudi Arabia VAT Registration Number.
    /// </summary>
    /// <param name="vat">The VAT number (15 digits starting with 3).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove SA prefix if present
        if (clean.StartsWith("SA", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!VatPattern().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Saudi Arabia VAT must be 15 digits starting with 3.");
        }

        // The last digit is a check digit using Luhn algorithm
        if (!ValidateLuhnChecksum(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Saudi Arabia VAT checksum.");
        }

        return ValidationResult.Success();
    }

    private static bool ValidateLuhnChecksum(string number)
    {
        int sum = 0;
        bool alternate = false;

        for (int i = number.Length - 1; i >= 0; i--)
        {
            int digit = number[i] - '0';

            if (alternate)
            {
                digit *= 2;
                if (digit > 9)
                {
                    digit -= 9;
                }
            }

            sum += digit;
            alternate = !alternate;
        }

        return sum % 10 == 0;
    }

    /// <summary>
    /// Gets details of a validated Saudi Arabia VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("SA", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "VAT Registration Number",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Saudi Arabia VAT Registration Number (رقم التسجيل الضريبي)"
        };
    }
}
