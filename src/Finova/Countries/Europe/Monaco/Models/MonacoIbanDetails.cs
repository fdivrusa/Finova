using Finova.Core.Models;

namespace Finova.Countries.Europe.Monaco.Models;

/// <summary>
/// Monaco specific IBAN details.
/// MC IBAN format: MC + 2 check + 5 Bank + 5 Branch + 11 Account + 2 RIB Key.
/// Same structure as France.
/// </summary>
public record MonacoIbanDetails : IbanDetails
{
    /// <summary>
    /// Code Banque (Bank Code - 5 digits).
    /// </summary>
    public required string CodeBanque { get; init; }

    /// <summary>
    /// Code Guichet (Branch Code - 5 digits).
    /// </summary>
    public required string CodeGuichet { get; init; }

    /// <summary>
    /// Numéro de Compte (Account Number - 11 alphanumeric).
    /// </summary>
    public required string NumeroCompte { get; init; }

    /// <summary>
    /// Clé RIB (RIB Key - 2 digits).
    /// Internal check digits between 01 and 97.
    /// </summary>
    public required string CleRib { get; init; }
}
