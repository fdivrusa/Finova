using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.UnitedKingdom.Validators
{
    /// <summary>
    /// Validator for United Kingdom bank accounts.
    /// United Kingdom IBAN format: GB + 2 check digits + 4 bank code + 6 sort code + 8 account number (22 characters total).
    /// Example: GB29NWBK60161331926819 or formatted: GB29 NWBK 6016 1331 9268 19
    /// </summary>
    public class UnitedKingdomIbanValidator : IIbanValidator
    {
        public string CountryCode => "GB";

        private const int UnitedKingdomIbanLength = 22;
        private const string UnitedKingdomCountryCode = "GB";

        #region Instance Methods (for Dependency Injection)
        public bool IsValidIban([NotNullWhen(true)] string? iban)
        {
            return ValidateUnitedKingdomIban(iban);
        }
        #endregion

        #region Static Methods (for Direct Usage)
        /// <summary>
        /// Validates a United Kingdom IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate.</param>
        /// <returns> True if the IBAN is valid for United Kingdom; otherwise, false.</returns>
        public static bool ValidateUnitedKingdomIban(string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }
            var normalized = IbanHelper.NormalizeIban(iban);
            // Check length (United Kingdom IBAN is exactly 22 characters)
            if (normalized.Length != UnitedKingdomIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(UnitedKingdomCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check that positions 4 to 7 are LETTERS
            for (int i = 4; i < 8; i++)
            {

                if (!char.IsLetter(normalized[i]))
                {
                    return false;
                }
            }

            // Check that positions 8 to 21 are DIGITS
            for (int i = 8; i < 22; i++)
            {
                if (!char.IsDigit(normalized[i]))
                {
                    return false;
                }
            }

            // Validate structure and checksum
            return IbanHelper.IsValidIban(normalized);
        }
        #endregion
    }
}
