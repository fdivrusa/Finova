using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Belgium.Validators
{
    /// <summary>
    /// Validator for Belgian IBAN bank accounts.
    /// Belgian IBAN format: BE + 2 check digits + 12 digits (16 characters total).
    /// Example: BE68539007547034 or formatted: BE68 5390 0754 7034
    /// </summary>
    public class BelgianBankAccountValidator : IBankAccountValidator
    {
        public string CountryCode => "BE";

        private const int BelgianIbanLength = 16;
        private const string BelgianCountryCode = "BE";

        #region Instance Methods (for Dependency Injection)

        public bool IsValidIban(string? iban)
        {
            return ValidateBelgianIban(iban);
        }

        #endregion

        #region Static Methods (for Direct Usage)

        /// <summary>
        /// Validates a Belgian IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate</param>
        /// <returns>True if valid Belgian IBAN, false otherwise</returns>
        public static bool ValidateBelgianIban([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            // Check length (Belgian IBAN is exactly 16 characters)
            if (normalized.Length != BelgianIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(BelgianCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Validate structure and checksum
            return IbanHelper.IsValidIban(normalized);
        }

        /// <summary>
        /// Formats a Belgian IBAN with spaces for display.
        /// </summary>
        /// <param name="iban">The IBAN to format</param>
        /// <returns>Formatted IBAN (e.g., "BE68 5390 0754 7034")</returns>
        public static string FormatBelgianIban(string? iban)
        {
            if (!ValidateBelgianIban(iban))
            {
                throw new ArgumentException("Invalid Belgian IBAN", nameof(iban));
            }

            return IbanHelper.FormatIban(iban);
        }

        #endregion
    }
}
