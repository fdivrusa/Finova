using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Iban;
using Finova.Core.Bic;
using Finova.Core.PaymentCard;
using Finova.Core.PaymentReference;
using Finova.Core.Vat;

using Finova.Countries.Europe.Italy.Models;

namespace Finova.Countries.Europe.Italy.Validators;

/// <summary>
/// Validator for Italian VAT numbers (Partita IVA).
/// Format: IT + 11 digits.
/// Structure: 7 digits (progressive number) + 3 digits (office code) + 1 digit (check).
/// Checksum: Luhn algorithm.
/// </summary>
public partial class ItalyVatValidator : IVatValidator
{
    [GeneratedRegex(@"[^\d]")]
    private static partial Regex DigitsOnlyRegex();

    private const string CountryCodePrefix = "IT";

    public string CountryCode => CountryCodePrefix;

    public ValidationResult Validate(string? vat) => ValidateItalyVat(vat);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult ValidateItalyVat(string? vat)
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
                return ValidationResult.Failure(ValidationErrorCode.InvalidCountryCode, "Invalid format. Expected IT prefix or 11 digits.");
            }
        }
        else
        {
            // Remove IT
            normalized = normalized[2..];
        }

        normalized = DigitsOnlyRegex().Replace(normalized, "");

        if (normalized.Length != 11)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid length. Expected 11 digits, got {normalized.Length}.");
        }

        if (normalized == "00000000000")
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "VAT number cannot be all zeros.");
        }

        // Luhn Check
        if (!LuhnCheck(normalized))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid checksum (Luhn).");
        }

        return ValidationResult.Success();
    }

    private static bool LuhnCheck(string digits)
    {
        int sum = 0;
        for (int i = 0; i < 11; i++)
        {
            int digit = digits[i] - '0';
            if (i % 2 == 0) // Odd position (1st, 3rd...) -> Index 0, 2...
            {
                sum += digit;
            }
            else // Even position (2nd, 4th...) -> Index 1, 3...
            {
                int doubled = digit * 2;
                if (doubled > 9) doubled -= 9;
                sum += doubled;
            }
        }

        return (sum % 10) == 0;
    }

    public static ItalyVatDetails? GetVatDetails(string? vat)
    {
        var result = ValidateItalyVat(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var normalized = vat!.Trim().ToUpperInvariant();
        if (normalized.StartsWith(CountryCodePrefix))
        {
            normalized = normalized[2..];
        }
        normalized = DigitsOnlyRegex().Replace(normalized, "");

        var officeCode = normalized.Substring(7, 3);

        return new ItalyVatDetails
        {
            VatNumber = $"{CountryCodePrefix}{normalized}",
            CountryCode = CountryCodePrefix,
            IsValid = true,
            OfficeCode = officeCode
        };
    }
}

