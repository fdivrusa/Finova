namespace Finova.Core.Vat;

/// <summary>
/// Represents the details of a parsed VAT number.
/// </summary>
public record VatDetails
{
    /// <summary>
    /// The full VAT number (including country code).
    /// </summary>
    public string VatNumber { get; init; } = string.Empty;

    /// <summary>
    /// The country code (e.g., "BE", "FR").
    /// </summary>
    public string CountryCode { get; init; } = string.Empty;

    /// <summary>
    /// Indicates if the VAT number is valid.
    /// </summary>
    public bool IsValid { get; init; }

    /// <summary>
    /// The kind of identifier (e.g., VAT, Business Tax ID, Invoicing Scheme).
    /// </summary>
    public string IdentifierKind { get; init; } = "Vat";

    /// <summary>
    /// Indicates if this is a formal EU VAT number.
    /// </summary>
    public bool IsEuVat { get; init; }

    /// <summary>
    /// Indicates if this number is eligible for VIES validation.
    /// </summary>
    public bool IsViesEligible { get; init; }

    /// <summary>
    /// Additional notes regarding the usage or semantics of this identifier.
    /// </summary>
    public string? Notes { get; init; }
}
