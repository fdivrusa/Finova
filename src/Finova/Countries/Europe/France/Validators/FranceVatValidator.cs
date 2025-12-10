using System.Diagnostics.CodeAnalysis;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.France.Models;

namespace Finova.Countries.Europe.France.Validators;

/// <summary>
/// Validator for French VAT numbers (TVA).
/// Format: FR + 2 check digits + 9 digits (SIREN).
/// Total length: 11 characters (excluding prefix) or 13 (including FR).
/// Checksum: Key = [12 + 3 * (SIREN % 97)] % 97.
/// </summary>
public class FranceVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "FR";

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? vat) => ValidateFranceVat(vat);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult ValidateFranceVat(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        var normalized = vat.Trim().ToUpperInvariant();

        // Check prefix
        if (!normalized.StartsWith(CountryCodePrefix))
        {
            if (normalized.Length == 11 && long.TryParse(normalized, out _))
            {
                // It's just the numbers, proceed
            }
            else
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid format. Expected FR prefix or 11 digits.");
            }
        }
        else
        {
            // Remove FR
            normalized = normalized[2..];
        }

        // Remove spaces/dots manually
        normalized = normalized.Replace(" ", "").Replace(".", "");

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected 11 digits, got {normalized.Length}.");
        }

        if (!long.TryParse(normalized, out _))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "VAT number must contain only digits after prefix.");
        }

        // Extract parts
        // Key is first 2 digits
        // SIREN is last 9 digits
        var keyStr = normalized.Substring(0, 2);
        var sirenStr = normalized.Substring(2, 9);

        if (!int.TryParse(keyStr, out int key) || !long.TryParse(sirenStr, out long siren))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid numeric format.");
        }

        // Checksum algorithm
        // Key = [12 + 3 * (SIREN % 97)] % 97
        long sirenMod97 = siren % 97;
        long calculatedKey = (12 + 3 * sirenMod97) % 97;

        if (key != calculatedKey)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum.");
        }

        return ValidationResult.Success();
    }

    public static FranceVatDetails? GetVatDetails(string? vat)
    {
        var result = ValidateFranceVat(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var normalized = vat!.Trim().ToUpperInvariant();
        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }
        normalized = normalized.Replace(" ", "").Replace(".", "");

        var siren = normalized.Substring(2, 9);

        return new FranceVatDetails
        {
            VatNumber = $"{CountryCodePrefix}{normalized}",
            CountryCode = CountryCodePrefix,
            IsValid = true,
            Siren = siren
        };
    }
}

