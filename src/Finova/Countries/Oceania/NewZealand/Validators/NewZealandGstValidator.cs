using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Oceania.NewZealand.Validators;

/// <summary>
/// Validates New Zealand Goods and Services Tax (GST) number.
/// Format: 8 or 9 digit IRD number.
/// The IRD (Inland Revenue Department) number is used for GST registration.
/// Format: XX-XXX-XXX or XXX-XXX-XXX (8-9 digits)
/// Checksum: Weighted sum mod 11
/// </summary>
public partial class NewZealandGstValidator : IVatValidator
{
    private const string CountryCodePrefix = "NZ";

    [GeneratedRegex(@"^\d{8,9}$")]
    private static partial Regex IrdRegex();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a New Zealand GST/IRD number.
    /// </summary>
    /// <param name="gst">The GST/IRD number string (8-9 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? gst)
    {
        if (string.IsNullOrWhiteSpace(gst))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = gst.Trim().Replace(" ", "").Replace("-", "");

        // Remove NZ prefix if present
        if (clean.StartsWith("NZ", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!IrdRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "IRD number must be 8 or 9 digits.");
        }

        // Pad to 9 digits if 8 digits
        if (clean.Length == 8)
        {
            clean = "0" + clean;
        }

        // Validate checksum using IRD algorithm
        // Weights: 3, 2, 7, 6, 5, 4, 3, 2 for first 8 digits
        int[] primaryWeights = { 3, 2, 7, 6, 5, 4, 3, 2 };
        int[] secondaryWeights = { 7, 4, 3, 2, 5, 2, 7, 6 };

        int sum = 0;
        for (int i = 0; i < 8; i++)
        {
            sum += (clean[i] - '0') * primaryWeights[i];
        }

        int remainder = sum % 11;
        int checkDigit;

        if (remainder == 0)
        {
            checkDigit = 0;
        }
        else
        {
            // Try with secondary weights
            int secondarySum = 0;
            for (int i = 0; i < 8; i++)
            {
                secondarySum += (clean[i] - '0') * secondaryWeights[i];
            }

            int secondaryRemainder = secondarySum % 11;
            if (secondaryRemainder == 0)
            {
                checkDigit = 0;
            }
            else
            {
                checkDigit = 11 - secondaryRemainder;
            }

            // If check digit is 10 after secondary, it's invalid
            if (checkDigit == 10)
            {
                // Fall back to primary calculation
                checkDigit = 11 - remainder;
                if (checkDigit == 10)
                {
                    return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
                }
            }
        }

        int actualCheckDigit = clean[8] - '0';

        // Also try the simpler mod11 calculation
        int simpleSum = 0;
        for (int i = 0; i < 8; i++)
        {
            simpleSum += (clean[i] - '0') * primaryWeights[i];
        }
        int simpleCheckDigit = 11 - (simpleSum % 11);
        if (simpleCheckDigit == 11) simpleCheckDigit = 0;
        if (simpleCheckDigit == 10)
        {
            // Use secondary weights
            int secSum = 0;
            for (int i = 0; i < 8; i++)
            {
                secSum += (clean[i] - '0') * secondaryWeights[i];
            }
            simpleCheckDigit = 11 - (secSum % 11);
            if (simpleCheckDigit == 11) simpleCheckDigit = 0;
        }

        if (actualCheckDigit != simpleCheckDigit)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Gets details of a validated New Zealand GST number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? gst)
    {
        if (!Validate(gst).IsValid)
        {
            return null;
        }

        var clean = gst!.Trim().Replace(" ", "").Replace("-", "");

        // Remove NZ prefix if present
        if (clean.StartsWith("NZ", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "GST",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "New Zealand Goods and Services Tax (IRD number)"
        };
    }
}
