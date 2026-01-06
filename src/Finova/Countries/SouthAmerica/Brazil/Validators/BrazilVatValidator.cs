using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.SouthAmerica.Brazil.Validators;

/// <summary>
/// Validates Brazilian VAT identifier (CNPJ).
/// Brazil does not have a traditional VAT system like Europe, but uses
/// CNPJ (Cadastro Nacional da Pessoa Jurídica) for business identification
/// which is used for tax purposes including ICMS (state sales tax),
/// IPI (federal excise tax), and ISS (municipal services tax).
/// </summary>
public class BrazilVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "BR";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Brazilian VAT/CNPJ number.
    /// </summary>
    /// <param name="vat">The CNPJ number string (14 digits).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "").Replace(".", "").Replace("/", "");

        // Remove BR prefix if present
        if (clean.StartsWith("BR", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Use the existing CNPJ validator
        return BrazilCnpjValidator.ValidateCnpj(clean);
    }

    /// <summary>
    /// Gets details of a validated Brazilian VAT/CNPJ number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "").Replace(".", "").Replace("/", "");

        // Remove BR prefix if present
        if (clean.StartsWith("BR", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "CNPJ",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = "Brazilian business tax identifier (CNPJ)"
        };
    }
}
