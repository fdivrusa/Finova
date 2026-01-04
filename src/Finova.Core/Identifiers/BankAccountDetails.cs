namespace Finova.Core.Identifiers;

/// <summary>
/// Represents the parsed details of a Bank Account Number.
/// </summary>
public record BankAccountDetails
{
    /// <summary>
    /// The original account number (normalized).
    /// </summary>
    public required string AccountNumber { get; init; }

    /// <summary>
    /// ISO country code (e.g., "US", "SG", "JP").
    /// </summary>
    public required string CountryCode { get; init; }

    /// <summary>
    /// The bank code (e.g., Code Banque in France/Belgium, BLZ in Germany).
    /// </summary>
    public string? BankCode { get; init; }

    /// <summary>
    /// The core account number (excluding check digits or branch codes if they are embedded).
    /// </summary>
    public string? CoreAccountNumber { get; init; }

    /// <summary>
    /// The check digit(s) extracted from the account number (if applicable).
    /// </summary>
    public string? CheckDigits { get; init; }

    /// <summary>
    /// The branch code extracted from the account number (if applicable).
    /// Also known as "Code Guichet" in France.
    /// </summary>
    public string? BranchCode { get; init; }

    /// <summary>
    /// The account type (if encoded in the number).
    /// </summary>
    public string? AccountType { get; init; }
}
