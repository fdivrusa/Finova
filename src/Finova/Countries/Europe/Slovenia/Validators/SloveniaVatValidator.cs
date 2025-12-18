using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Enterprise;
using Finova.Core.Identifiers;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Slovenia.Validators;

/// <summary>
/// Validator for Slovenia VAT numbers (ID za DDV).
/// Format: SI + 8 digits.
/// Algorithm: Weighted Modulo 11.
/// </summary>
public partial class SloveniaVatValidator : IVatValidator, ITaxIdValidator
{
    [GeneratedRegex(@"^\d{8}$")]
    private static partial Regex VatRegex();

    private const string VatPrefix = "SI";
    private static readonly int[] Weights = [8, 7, 6, 5, 4, 3, 2];

    public string CountryCode => VatPrefix;

    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    public ValidationResult Validate(string? number) => ValidateVat(number);

    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    string? IValidator<string>.Parse(string? instance) => Normalize(instance);

    public static ValidationResult ValidateVat(string? vat)
    {
        vat = VatSanitizer.Sanitize(vat);

        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var cleaned = vat.Trim().ToUpperInvariant();
        if (cleaned.StartsWith(VatPrefix))
        {
            cleaned = cleaned[2..];
        }

        if (!VatRegex().IsMatch(cleaned))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidSloveniaVatFormat);
        }

        int sum = ChecksumHelper.CalculateWeightedSum(cleaned[..7], Weights);
        int remainder = sum % 11;

        if (remainder == 1)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSloveniaVatChecksumForbidden);
        }

        int checkDigit = remainder == 0 ? 0 : 11 - remainder;

        if (checkDigit != (cleaned[7] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidSloveniaVatChecksum);
        }

        return ValidationResult.Success();
    }

    public static VatDetails? GetVatDetails(string? vat)
    {
        var result = ValidateVat(vat);
        if (!result.IsValid)
        {
            return null;
        }

        var cleaned = VatSanitizer.Sanitize(vat)!.Trim().ToUpperInvariant();
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

    public static string? Normalize(string? number)
    {
        if (string.IsNullOrWhiteSpace(number)) return null;
        var cleaned = number.ToUpperInvariant().Replace(VatPrefix, "").Replace(" ", "");
        return VatRegex().IsMatch(cleaned) ? cleaned : null;
    }
}