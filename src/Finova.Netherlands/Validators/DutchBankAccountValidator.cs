using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Netherlands.Validators
{
    /// <summary>
    /// Validator for Dutch (Netherlands) IBAN bank accounts.
    /// Dutch IBAN format: NL + 2 check digits + 4 bank code + 10 account number (18 characters total).
    /// Example: NL91ABNA0417164300 or formatted: NL91 ABNA 0417 1643 00
    /// </summary>
    public class DutchBankAccountValidator : IBankAccountValidator
    {
        public string CountryCode => "NL";

        private const int DutchIbanLength = 18;
        private const string DutchCountryCode = "NL";

        #region Instance Methods (for Dependency Injection)

        public bool IsValidIban(string? iban)
        {
            return ValidateDutchIban(iban);
        }

        #endregion

        #region Static Methods (for Direct Usage)

        /// <summary>
        /// Validates a Dutch IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate</param>
        /// <returns>True if valid Dutch IBAN, false otherwise</returns>
        public static bool ValidateDutchIban([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            // Check length (Dutch IBAN is exactly 18 characters)
            if (normalized.Length != DutchIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(DutchCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check bank code format (4 letters after country code and check digits)
            // Position 4-7 should be letters (bank code like ABNA, RABO, INGB, etc.)
            var bankCode = normalized.Substring(4, 4);
            if (!bankCode.All(char.IsLetter))
            {
                return false;
            }

            // Check account number format (10 digits after bank code)
            // Position 8-17 should be digits
            var accountNumber = normalized.Substring(8, 10);
            if (!accountNumber.All(char.IsDigit))
            {
                return false;
            }

            // Validate IBAN checksum
            return IbanHelper.IsValidIban(normalized);
        }

        /// <summary>
        /// Extracts the bank code from a Dutch IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to extract from</param>
        /// <returns>The 4-character bank code (e.g., "ABNA", "RABO")</returns>
        public static string? GetBankCode(string? iban)
        {
            if (!ValidateDutchIban(iban))
            {
                return null;
            }

            var normalized = IbanHelper.NormalizeIban(iban);
            return normalized.Substring(4, 4);
        }

        /// <summary>
        /// Formats a Dutch IBAN with spaces for display.
        /// </summary>
        /// <param name="iban">The IBAN to format</param>
        /// <returns>Formatted IBAN (e.g., "NL91 ABNA 0417 1643 00")</returns>
        public static string FormatDutchIban(string? iban)
        {
            if (!ValidateDutchIban(iban))
            {
                throw new ArgumentException("Invalid Dutch IBAN", nameof(iban));
            }

            return IbanHelper.FormatIban(iban);
        }

        #endregion
    }
}