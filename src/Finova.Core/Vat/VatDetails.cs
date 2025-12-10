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
}
