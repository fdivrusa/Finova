using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Chile.Validators;

/// <summary>
/// Validates Chilean VAT identifier (RUT - Rol Único Tributario).
/// Chile uses RUT for VAT (IVA) registration purposes.
/// Format: XX.XXX.XXX-X (8-9 digits including check digit).
/// </summary>
public class ChileVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "CL";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Chilean VAT/RUT number.
    /// </summary>
    /// <param name="vat">The RUT number (8-9 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace(".", "").Replace("-", "");

        // Remove CL prefix if present
        if (clean.StartsWith("CL", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Use the existing RUT validator
        return ChileRutValidator.ValidateStatic(clean);
    }

    /// <summary>
    /// Gets details of a validated Chilean VAT/RUT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace(".", "").Replace("-", "").ToUpperInvariant();

        // Remove CL prefix if present
        if (clean.StartsWith("CL", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "RUT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Chilean tax identifier (RUT)"
        };
    }
}
