using Finova.Core.Iban;

namespace Finova.Countries.Europe.Iceland.Models;

/// <summary>
/// Iceland specific IBAN details.
/// IS IBAN format: IS + 2 check + 4 Bank + 2 Branch + 6 Account + 10 Kennitala (National ID).
/// </summary>
public record IcelandIbanDetails : IbanDetails
{
    /// <summary>
    /// Banki (Bank Code - 4 digits).
    /// </summary>
    public required string Banki { get; init; }

    /// <summary>
    /// Hb (Branch Code - 2 digits).
    /// </summary>
    public required string Hb { get; init; }

    /// <summary>
    /// Reikningsnumer (Account Number - 6 digits).
    /// </summary>
    public required string Reikningsnumer { get; init; }

    /// <summary>
    /// Kennitala (National ID - 10 digits).
    /// Identifies the account holder.
    /// </summary>
    public required string Kennitala { get; init; }
}
