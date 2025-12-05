using Finova.Core.Models;

namespace Finova.Countries.Europe.Andorra.Models;

/// <summary>
/// Andorra specific IBAN details.
/// AD IBAN format: AD + 2 check + 4 Bank + 4 Branch + 12 Account.
/// </summary>
public record AndorraIbanDetails : IbanDetails
{
    /// <summary>
    /// Codi d'entitat (Bank Code - 4 digits).
    /// </summary>
    public required string CodiEntitat { get; init; }

    /// <summary>
    /// Codi d'oficina (Branch Code - 4 digits).
    /// </summary>
    public required string CodiOficina { get; init; }

    /// <summary>
    /// Número de compte (Account Number - 12 alphanumeric).
    /// </summary>
    public required string NumeroCompte { get; init; }
}
