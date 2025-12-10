using Finova.Core.Vat;


namespace Finova.Countries.Europe.Belgium.Models;

/// <summary>
/// Belgian-specific VAT details.
/// </summary>
public record BelgiumVatDetails : VatDetails
{
    /// <summary>
    /// The Enterprise Number (KBO/BCE) associated with this VAT number.
    /// </summary>
    public string? EnterpriseNumber { get; init; }
}

