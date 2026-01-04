using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.MiddleEast.Israel.Validators;

/// <summary>
/// Validates Israeli VAT Registration Number (מספר עוסק מורשה).
/// Format: 9 digits with Luhn checksum.
/// </summary>
public partial class IsraelVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "IL";

    [GeneratedRegex(@"^\d{9}$", RegexOptions.Compiled)]
    private static partial Regex VatPattern();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates an Israeli VAT Registration Number.
    /// </summary>
    /// <param name="vat">The VAT number (9 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove IL prefix if present
        if (clean.StartsWith("IL", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Pad with leading zeros if necessary (some older numbers have less digits)
        if (clean.Length < 9 && clean.Length >= 5)
        {
            clean = clean.PadLeft(9, '0');
        }

        if (!VatPattern().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Israeli VAT must be 9 digits.");
        }

        // Validate using Luhn algorithm variant
        if (!ValidateChecksum(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Israeli VAT checksum.");
        }

        return ValidationResult.Success();
    }

    private static bool ValidateChecksum(string number)
    {
        // Israeli ID/VAT uses a modified Luhn algorithm
        int sum = 0;
        for (int i = 0; i < 9; i++)
        {
            int digit = number[i] - '0';
            int weight = (i % 2 == 0) ? 1 : 2;
            int product = digit * weight;
            
            if (product > 9)
            {
                product -= 9;
            }
            
            sum += product;
        }

        return sum % 10 == 0;
    }

    /// <summary>
    /// Gets details of a validated Israeli VAT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "");

        if (clean.StartsWith("IL", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (clean.Length < 9)
        {
            clean = clean.PadLeft(9, '0');
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "Authorized Dealer Number",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Israeli VAT Registration Number (מספר עוסק מורשה)"
        };
    }
}
