using Finova.Core.Enterprise;

namespace Finova.Services;

/// <summary>
/// Provides routing information for European identifiers.
/// </summary>
public record EuropeIdentifierSupport
{
    /// <summary>
    /// Indicates if VAT validation is supported and makes sense for this country.
    /// </summary>
    public bool SupportsVatValidation { get; init; }

    /// <summary>
    /// Indicates if Enterprise validation is supported and makes sense for this country.
    /// </summary>
    public bool SupportsEnterpriseValidation { get; init; }

    /// <summary>
    /// The recommended Enterprise Number Type for this country, if applicable.
    /// </summary>
    public EnterpriseNumberType? RecommendedEnterpriseNumberType { get; init; }

    /// <summary>
    /// The semantic kind of the VAT identifier (e.g., VAT, BusinessTaxId).
    /// </summary>
    public IdentifierKind VatKind { get; init; }

    /// <summary>
    /// Additional notes regarding the usage or semantics of identifiers in this country.
    /// </summary>
    public string? Notes { get; init; }
}

/// <summary>
/// Helper class to determine the correct validation strategy for a given country.
/// </summary>
public static class EuropeIdentifierRouting
{
    /// <summary>
    /// Gets the support and routing information for a specific country code.
    /// </summary>
    /// <param name="countryCode">The 2-letter ISO country code (e.g., "BE", "FR").</param>
    /// <returns>A <see cref="EuropeIdentifierSupport"/> object containing routing details.</returns>
    public static EuropeIdentifierSupport GetSupport(string countryCode)
    {
        var metadata = EuropeCountryIdentifierMetadata.For(countryCode);

        if (metadata == null)
        {
            return new EuropeIdentifierSupport
            {
                SupportsVatValidation = false,
                SupportsEnterpriseValidation = false,
                RecommendedEnterpriseNumberType = null,
                VatKind = IdentifierKind.NotApplicable,
                Notes = "Country not supported or unknown."
            };
        }

        return new EuropeIdentifierSupport
        {
            SupportsVatValidation = metadata.SupportedScopes.HasFlag(IdentifierScope.Vat),
            SupportsEnterpriseValidation = metadata.SupportedScopes.HasFlag(IdentifierScope.Enterprise),
            RecommendedEnterpriseNumberType = metadata.DefaultEnterpriseType,
            VatKind = metadata.VatKind,
            Notes = metadata.Note
        };
    }
}
