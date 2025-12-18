namespace Finova.Core.Identifiers;

/// <summary>
/// Represents the parsed details of a Bank Routing Number.
/// </summary>
public record BankRoutingDetails
{
    /// <summary>
    /// The original routing number (normalized).
    /// </summary>
    public required string RoutingNumber { get; init; }

    /// <summary>
    /// ISO country code (e.g., "US", "CA", "AU").
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// The bank code/identifier extracted from the routing number.
    /// </summary>
    public string? BankCode { get; init; }

    /// <summary>
    /// The branch code extracted from the routing number (if applicable).
    /// </summary>
    public string? BranchCode { get; init; }

    /// <summary>
    /// The district or region code (if applicable, e.g., US Federal Reserve District).
    /// </summary>
    public string? DistrictCode { get; init; }

    /// <summary>
    /// The check digit(s) extracted from the routing number.
    /// </summary>
    public string? CheckDigits { get; init; }
}
