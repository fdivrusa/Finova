using System.Text.RegularExpressions;
using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Colombia.Validators;

/// <summary>
/// Validates Colombian NIT (Número de Identificación Tributaria).
/// Used for VAT (IVA) registration purposes.
/// Format: 9 digits + check digit (total 10 digits).
/// </summary>
public partial class ColombiaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "CO";

    [GeneratedRegex(@"^\d{9,10}$", RegexOptions.Compiled)]
    private static partial Regex NitPattern();

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Colombian NIT number.
    /// </summary>
    /// <param name="vat">The NIT number (9-10 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "").Replace(".", "");

        // Remove CO prefix if present
        if (clean.StartsWith("CO", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        if (!NitPattern().IsMatch(clean))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidColombianNitFormat);
        }

        // If 9 digits, no check digit validation (just the base number)
        if (clean.Length == 9)
        {
            return ValidationResult.Success();
        }

        // Validate checksum for 10 digit NIT
        // Weights: 41, 37, 29, 23, 19, 17, 13, 7, 3
        int[] weights = [41, 37, 29, 23, 19, 17, 13, 7, 3];
        int sum = 0;

        for (int i = 0; i < 9; i++)
        {
            sum += (clean[i] - '0') * weights[i];
        }

        int remainder = sum % 11;
        int checkDigit = remainder > 1 ? 11 - remainder : remainder;

        if (checkDigit != (clean[9] - '0'))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidColombianNitChecksum);
        }

        return ValidationResult.Success();
    }

    /// <summary>
    /// Gets details of a validated Colombian NIT.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "").Replace(".", "");

        if (clean.StartsWith("CO", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "NIT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Colombian tax identifier (NIT)"
        };
    }
}
