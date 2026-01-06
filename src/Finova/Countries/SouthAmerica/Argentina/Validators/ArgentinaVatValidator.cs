using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Argentina.Validators;

/// <summary>
/// Validates Argentine VAT identifier (CUIT - Código Único de Identificación Tributaria).
/// Argentina uses CUIT for VAT (IVA) registration purposes.
/// Format: XX-XXXXXXXX-X (11 digits with optional hyphens).
/// </summary>
public class ArgentinaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "AR";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates an Argentine VAT/CUIT number.
    /// </summary>
    /// <param name="vat">The CUIT number (11 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove AR prefix if present
        if (clean.StartsWith("AR", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Use the existing CUIT validator
        return ArgentinaCuitValidator.ValidateStatic(clean);
    }

    /// <summary>
    /// Gets details of a validated Argentine VAT/CUIT number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "");

        // Remove AR prefix if present
        if (clean.StartsWith("AR", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // First two digits indicate entity type
        var prefix = clean[..2];
        var entityType = prefix switch
        {
            "20" or "23" or "24" or "27" => "Individual",
            "30" or "33" or "34" => "Company",
            _ => "Unknown"
        };

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "CUIT",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = $"Argentine tax identifier (CUIT) - {entityType}"
        };
    }
}
