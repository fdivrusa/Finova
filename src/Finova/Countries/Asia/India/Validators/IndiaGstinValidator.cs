using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.India.Validators;

/// <summary>
/// Validates Indian Goods and Services Tax Identification Number (GSTIN).
/// Format: 22AAAAA0000A1Z5 (15 characters)
/// - Positions 1-2: State code (01-37)
/// - Positions 3-12: PAN of the taxpayer
/// - Position 13: Entity number (1-9, A-Z)
/// - Position 14: 'Z' (default)
/// - Position 15: Checksum character
/// </summary>
public partial class IndiaGstinValidator : IVatValidator
{
    private const string CountryCodePrefix = "IN";

    // GSTIN format: 2 digits state + 10 char PAN + 1 entity + 1 'Z' + 1 checksum
    [GeneratedRegex(@"^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z][0-9A-Z]Z[0-9A-Z]$")]
    private static partial Regex GstinRegex();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates an Indian GSTIN.
    /// </summary>
    /// <param name="gstin">The GSTIN string (15 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? gstin)
    {
        if (string.IsNullOrWhiteSpace(gstin))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = gstin.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");

        if (clean.Length != 15)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidLength, "GSTIN must be 15 characters.");
        }

        if (!GstinRegex().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid GSTIN format.");
        }

        // Validate state code (01-37)
        if (!int.TryParse(clean[..2], out int stateCode) || stateCode < 1 || stateCode > 37)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid state code in GSTIN.");
        }

        // Extract the embedded PAN and validate its structure
        string embeddedPan = clean.Substring(2, 10);
        if (!ValidatePanStructure(embeddedPan))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid PAN embedded in GSTIN.");
        }

        // Position 14 must be 'Z' (default)
        if (clean[13] != 'Z')
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Position 14 in GSTIN must be 'Z'.");
        }

        // Validate checksum (position 15)
        char expectedChecksum = CalculateGstinChecksum(clean[..14]);
        if (clean[14] != expectedChecksum)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Validates the structure of a PAN embedded in GSTIN.
    /// </summary>
    private static bool ValidatePanStructure(string pan)
    {
        if (pan.Length != 10)
        {
            return false;
        }

        // First 5 characters: letters
        for (int i = 0; i < 5; i++)
        {
            if (!char.IsLetter(pan[i]))
            {
                return false;
            }
        }

        // Next 4 characters: digits
        for (int i = 5; i < 9; i++)
        {
            if (!char.IsDigit(pan[i]))
            {
                return false;
            }
        }

        // Last character: letter
        if (!char.IsLetter(pan[9]))
        {
            return false;
        }

        // 4th character validation (Status)
        char status = pan[3];
        string validStatuses = "PCHABGJLFT";
        return validStatuses.Contains(status);
    }

    /// <summary>
    /// Calculates the GSTIN checksum character using the modified Luhn algorithm.
    /// </summary>
    private static char CalculateGstinChecksum(string input)
    {
        // Character set for GSTIN (0-9, A-Z)
        const string charSet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        int factor = 2;
        int sum = 0;
        int mod = charSet.Length;

        for (int i = input.Length - 1; i >= 0; i--)
        {
            int codePoint = charSet.IndexOf(input[i]);
            int addend = factor * codePoint;

            factor = factor == 2 ? 1 : 2;
            addend = (addend / mod) + (addend % mod);
            sum += addend;
        }

        int remainder = sum % mod;
        int checkCodePoint = (mod - remainder) % mod;

        return charSet[checkCodePoint];
    }

    /// <summary>
    /// Gets details of a validated GSTIN.
    /// </summary>
    public static VatDetails? GetVatDetails(string? gstin)
    {
        if (!Validate(gstin).IsValid)
        {
            return null;
        }

        var normalized = gstin!.Trim().ToUpperInvariant().Replace(" ", "").Replace("-", "");

        return new VatDetails
        {
            VatNumber = normalized,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "GSTIN",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Indian Goods and Services Tax Identification Number"
        };
    }
}
