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
    public class NetherlandsIbanValidator : IIbanValidator
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

            // Check Bank Code: Positions 4-7 must be letters (e.g., ABNA, INGB)
            for (int i = 4; i < 8; i++)
            {
                if (!char.IsLetter(normalized[i])) return false;
            }

            // Check Account Number: Positions 8-17 must be digits
            for (int i = 8; i < 18; i++)
            {
                if (!char.IsDigit(normalized[i])) return false;
            }

            // Validate IBAN checksum
            return IbanHelper.IsValidIban(normalized);
        }
        #endregion
    }
}