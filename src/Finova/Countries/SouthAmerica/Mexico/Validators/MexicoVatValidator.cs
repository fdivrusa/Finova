using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Mexico.Validators;

/// <summary>
/// Validates Mexican VAT identifier (RFC - Registro Federal de Contribuyentes).
/// Mexico uses RFC for both income tax and IVA (Impuesto al Valor Agregado) purposes.
/// Format: 12 characters for companies, 13 characters for individuals.
/// </summary>
public class MexicoVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "MX";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Mexican VAT/RFC number.
    /// </summary>
    /// <param name="vat">The RFC number (12-13 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove MX prefix if present
        if (clean.StartsWith("MX", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Use the existing RFC validator
        return MexicoRfcValidator.ValidateStatic(clean);
    }

    /// <summary>
    /// Gets details of a validated Mexican VAT/RFC number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "").ToUpperInvariant();

        // Remove MX prefix if present
        if (clean.StartsWith("MX", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        var entityType = clean.Length == 12 ? "Company" : "Individual";

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "RFC",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = $"Mexican tax identifier (RFC) - {entityType}"
        };
    }
}
