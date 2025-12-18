namespace Finova.Core.Identifiers;

/// <summary>
/// Interface for parsing Bank Account Numbers into detailed components.
/// </summary>
public interface IBankAccountParser
{
    /// <summary>
    /// ISO country code this parser handles (e.g., "US", "SG").
    /// </summary>
    string CountryCode { get; }

    /// <summary>
    /// Parses a bank account number and returns detailed information.
    /// </summary>
    /// <param name="accountNumber">The account number to parse.</param>
    /// <returns>
    /// <see cref="BankAccountDetails"/> with extracted components if valid;
    /// otherwise, null.
    /// </returns>
    BankAccountDetails? ParseBankAccount(string? accountNumber);
}
