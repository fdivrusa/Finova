using Finova.Core.Vat;


namespace Finova.Countries.Europe.Italy.Models;

/// <summary>
/// Italian-specific VAT details (Partita IVA).
/// </summary>
public record ItalyVatDetails : VatDetails
{
    /// <summary>
    /// The Office Code (digits 8-10) indicating the local tax office.
    /// </summary>
    public string? OfficeCode { get; init; }
}

