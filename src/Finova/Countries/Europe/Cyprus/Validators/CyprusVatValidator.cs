using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Europe.Cyprus.Validators;

public partial class CyprusVatValidator : IVatValidator
{
    [GeneratedRegex(@"^\d{8}[A-Z]$")]
    private static partial Regex VatRegex();

    // Mapping table for even positions (0, 2, 4, 6)
    // 0->1, 1->0, 2->5, 3->7, 4->9, 5->13, 6->15, 7->17, 8->19, 9->21
    private static readonly int[] CyprusMap = [1, 0, 5, 7, 9, 13, 15, 17, 19, 21];

    private const string VatPrefix = "CY";

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
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid Cyprus VAT format.");
        }

        // Checksum Validation
        // Format: 12345678 L
        string digits = cleaned.Substring(0, 8);
        char expectedLetter = cleaned[8];

        int sum = 0;

        for (int i = 0; i < 8; i++)
        {
            int digit = digits[i] - '0';

            // Even positions (0, 2, 4...) use mapping
            if (i % 2 == 0)
            {
                sum += CyprusMap[digit];
            }
            else
            {
                sum += digit;
            }
        }

        int remainder = sum % 26;
        char calculatedLetter = (char)(remainder + 'A');

        if (calculatedLetter != expectedLetter)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid Cyprus checksum.");
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
