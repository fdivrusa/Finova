namespace Finova.Core.Identifiers;

/// <summary>
/// Represents the parsed details of a BBAN (Basic Bank Account Number).
/// </summary>
public record BbanDetails
{
    /// <summary>
    /// The original BBAN (normalized/sanitized).
    /// </summary>
    public required string Bban { get; init; }

    /// <summary>
    /// ISO 3166-1 alpha-2 country code (e.g., "BE", "FR", "DE").
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// The bank code extracted from the BBAN (if applicable).
    /// </summary>
    public string? BankCode { get; init; }

    /// <summary>
    /// The branch code extracted from the BBAN (if applicable).
    /// </summary>
    public string? BranchCode { get; init; }

    /// <summary>
    /// The account number extracted from the BBAN.
    /// </summary>
    public string? AccountNumber { get; init; }

    /// <summary>
    /// The national check digit(s) extracted from the BBAN (if applicable).
    /// </summary>
    public string? NationalCheckDigits { get; init; }
}
