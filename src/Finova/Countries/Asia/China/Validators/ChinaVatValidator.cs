using Finova.Core.Common;
using Finova.Core.Vat;

namespace Finova.Countries.Asia.China.Validators;

/// <summary>
/// Validates Chinese VAT identifier (Unified Social Credit Code - USCC).
/// China uses USCC (统一社会信用代码) for VAT registration purposes.
/// Format: 18 alphanumeric characters.
/// </summary>
public class ChinaVatValidator : IVatValidator
{
    private const string CountryCodePrefix = "CN";

    /// <inheritdoc/>
    public string CountryCode => CountryCodePrefix;

    /// <inheritdoc/>
    ValidationResult IValidator<VatDetails>.Validate(string? instance) => Validate(instance);

    /// <inheritdoc/>
    public VatDetails? Parse(string? vat) => GetVatDetails(vat);

    /// <summary>
    /// Validates a Chinese VAT/USCC number.
    /// </summary>
    /// <param name="vat">The USCC number (18 characters).</param>
    /// <returns>A ValidationResult indicating success or failure.</returns>
    public static ValidationResult Validate(string? vat)
    {
        if (string.IsNullOrWhiteSpace(vat))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        var clean = vat.Trim().Replace(" ", "").Replace("-", "");

        // Remove CN prefix if present
        if (clean.StartsWith("CN", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // Use the existing USCC validator
        return ChinaUnifiedSocialCreditCodeValidator.ValidateUscc(clean);
    }

    /// <summary>
    /// Gets details of a validated Chinese VAT/USCC number.
    /// </summary>
    public static VatDetails? GetVatDetails(string? vat)
    {
        if (!Validate(vat).IsValid)
        {
            return null;
        }

        var clean = vat!.Trim().Replace(" ", "").Replace("-", "").ToUpperInvariant();

        // Remove CN prefix if present
        if (clean.StartsWith("CN", StringComparison.OrdinalIgnoreCase))
        {
            clean = clean[2..];
        }

        // First character indicates registration authority
        // Second character indicates entity type
        var entityType = clean[1] switch
        {
            '1' => "Institution (机构)",
            '2' => "Foreign-invested enterprise (外商投资企业)",
            '3' => "Enterprise (企业)",
            '4' => "Social organization (社会组织)",
            '5' => "Civilian-run non-enterprise unit (民办非企业单位)",
            '6' => "Foundation (基金会)",
            '7' => "Sole proprietorship (个体工商户)",
            '9' => "Other organization (其他组织)",
            _ => "Unknown"
        };

        return new VatDetails
        {
            VatNumber = clean,
            CountryCode = CountryCodePrefix,
            IsValid = true,
            IdentifierKind = "USCC",
            IsEuVat = false,
            IsViesEligible = false,
            Notes = $"Chinese VAT registration - {entityType}"
        };
    }
}
