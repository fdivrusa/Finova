using Finova.Core.Common;

namespace Finova.Core.Identifiers;

/// <summary>
/// Service for validating Bank Account Numbers (non-IBAN).
/// </summary>
public interface IBankAccountService
{
    /// <summary>
    /// Validates a bank account number for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO country code.</param>
    /// <param name="account">The bank account number.</param>
    /// <returns>A <see cref="ValidationResult"/>.</returns>
    ValidationResult Validate(string countryCode, string account);

    /// <summary>
    /// Parses a bank account number for a specific country.
    /// </summary>
    /// <param name="countryCode">The ISO country code.</param>
    /// <param name="account">The bank account number.</param>
    /// <returns>Parsed details or null.</returns>
    BankAccountDetails? Parse(string countryCode, string account);
}
