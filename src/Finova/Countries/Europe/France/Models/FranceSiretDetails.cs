namespace Finova.Countries.Europe.France.Models;

/// <summary>
/// Represents the details of a parsed French SIRET number.
/// </summary>
public class FranceSiretDetails
{
    /// <summary>
    /// The full 14-digit SIRET number.
    /// </summary>
    public string Siret { get; set; } = string.Empty;

    /// <summary>
    /// The 9-digit SIREN number (first 9 digits of SIRET).
    /// </summary>
    public string Siren { get; set; } = string.Empty;

    /// <summary>
    /// The 5-digit NIC (Numéro Interne de Classement) (last 5 digits of SIRET).
    /// </summary>
    public string Nic { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if the SIRET is valid.
    /// </summary>
    public bool IsValid { get; set; }
}
