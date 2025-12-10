using Finova.Core.Iban;


namespace Finova.Countries.Europe.FaroeIslands.Models;

/// <summary>
/// Faroe Islands-specific IBAN details.
/// FO IBAN format: FO + 2 check digits + 4 bank code + 9 account number + 1 check digit.
/// Example: FO7162646000016316
/// </summary>
public record FaroeIslandsIbanDetails : IbanDetails
{
    /// <summary>
    /// Gets the 4-digit bank code (Skrásetingarnummar).
    /// </summary>
    public required string SkrasetingarNummar { get; init; }

    /// <summary>
    /// Gets the 9-digit account number (Kontonummar).
    /// </summary>
    public required string KontoNummar { get; init; }

    /// <summary>
    /// Gets the 1-digit control number (Eftirlitstal).
    /// </summary>
    public required string EftirlitsTal { get; init; }
}

