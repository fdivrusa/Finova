using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Luxembourg.Validators
{
    /// <summary>
    /// Validator for Luxembourg IBAN bank accounts.
    /// Luxembourg IBAN format: LU + 2 check digits + 3 bank code + 13 account number (20 characters total).
    /// Example: LU280019400644750000 or formatted: LU28 0019 4006 4475 0000
    /// </summary>
    public class LuxembourgBankAccountValidator : IBankAccountValidator
    {
        public string CountryCode => "LU";

        private const int LuxembourgIbanLength = 20;
        private const string LuxembourgCountryCode = "LU";

        #region Instance Methods (for Dependency Injection)

        public bool IsValidIban(string? iban)
        {
            return ValidateLuxembourgIban(iban);
        }

        #endregion

        #region Static Methods (for Direct Usage)

        /// <summary>
        /// Validates a Luxembourg IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate</param>
        /// <returns>True if valid Luxembourg IBAN, false otherwise</returns>
        public static bool ValidateLuxembourgIban([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            // Check length (Luxembourg IBAN is exactly 20 characters)
            if (normalized.Length != LuxembourgIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(LuxembourgCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Validate structure and checksum
            return IbanHelper.IsValidIban(normalized);
        }

        /// <summary>
        /// Formats a Luxembourg IBAN with spaces for display.
        /// </summary>
        /// <param name="iban">The IBAN to format</param>
        /// <returns>Formatted IBAN (e.g., "LU28 0019 4006 4475 0000")</returns>
        public static string FormatLuxembourgIban(string? iban)
        {
            if (!ValidateLuxembourgIban(iban))
            {
                throw new ArgumentException("Invalid Luxembourg IBAN", nameof(iban));
            }

            return IbanHelper.FormatIban(iban);
        }

        #endregion
    }
}
