using Finova.Core.Vat;


namespace Finova.Countries.Europe.France.Models;

/// <summary>
/// French-specific VAT details.
/// </summary>
public record FranceVatDetails : VatDetails
{
    /// <summary>
    /// The SIREN number (9 digits) extracted from the VAT number.
    /// </summary>
    public string? Siren { get; init; }
}

