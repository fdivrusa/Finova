using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.BosniaAndHerzegovina.Validators;

/// <summary>
/// Validator for Bosnia and Herzegovina VAT numbers.
/// Format: BA + 13 digits.
/// </summary>
public partial class BosniaAndHerzegovinaVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{13}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "BA";

    // Weights repeat: 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2
    private static readonly int[] Weights = { 7, 6, 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    public static ValidationResult Validate(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "VAT number cannot be empty.");
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Bosnia and Herzegovina VAT format.");
        }

        // Implementation of Modulo 11
        // Last digit is check digit
        int checkDigit = cleaned[12] - '0';
        string dataPart = cleaned.Substring(0, 12);

        int sum = ChecksumHelper.CalculateWeightedSum(dataPart, Weights);
        int remainder = sum % 11;

        int calculatedCheck;
        if (remainder == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Bosnia and Herzegovina VAT checksum (Remainder 1).");
        }
        else if (remainder == 0)
        {
            calculatedCheck = 0;
        }
        else
        {
            calculatedCheck = 11 - remainder;
        }

        if (calculatedCheck != checkDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Bosnia and Herzegovina VAT checksum.");
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

        var cleaned = vat!.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        return new VatDetails
        {
            CountryCode = VatPrefix,
            VatNumber = cleaned,
            IsValid = true
        };
    }
}