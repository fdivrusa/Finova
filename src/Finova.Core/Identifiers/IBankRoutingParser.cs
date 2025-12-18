namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for parsing Bank Routing Numbers into detailed components.
/// </summary>
public interface IBankRoutingParser
{
    /// <summary>
    /// ISO country code this parser handles (e.g., "US", "CA").
    /// </summary>
    string CountryCode { get; }

    /// <summary>
    /// Parses a routing number and returns detailed information.
    /// </summary>
    /// <param name="routingNumber">The routing number to parse.</param>
    /// <returns>
    /// <see cref="BankRoutingDetails"/> with extracted components if valid;
    /// otherwise, null.
    /// </returns>
    BankRoutingDetails? ParseRoutingNumber(string? routingNumber);
}
