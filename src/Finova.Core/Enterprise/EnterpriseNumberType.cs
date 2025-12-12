namespace Finova.Core.Enterprise;

/// <summary>
/// Types of Enterprise/Business Registration Numbers.
/// </summary>
public enum EnterpriseNumberType
{
    /// <summary>
    /// Austrian Business Register Number (Firmenbuchnummer).
    /// </summary>
    AustriaFirmenbuch,

    /// <summary>
    /// Belgian Enterprise Number (KBO/BCE).
    /// </summary>
    BelgiumEnterpriseNumber,

    /// <summary>
    /// French SIREN Number (9 digits).
    /// </summary>
    FranceSiren,

    /// <summary>
    /// French SIRET Number (14 digits).
    /// </summary>
    FranceSiret
}
