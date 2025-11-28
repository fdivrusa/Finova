using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.France.Validators
{
    /// <summary>
    /// Validator for France bank accounts.
    /// France IBAN format: FR + 2 check digits + 5 bank code + 5 branch code + 11 account number + 2 RIB key (27 characters total).
    /// Example: FR1420041010050500013M02606 or formatted: FR14 2004 1010 0505 0001 3M02 606
    /// </summary>
    public class FranceIbanValidator : IIbanValidator
    {
        public string CountryCode => "FR";

        private const int FranceIbanLength = 27;
        private const string FranceCountryCode = "FR";

        #region Instance Methods (for Dependency Injection)
        public bool IsValidIban([NotNullWhen(true)] string? iban)
        {
            return ValidateFranceIban(iban);
        }
        #endregion

        #region Static Methods (for Direct Usage)
        /// <summary>
        /// Validates a France IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate.</param>
        /// <returns> True if the IBAN is valid for France; otherwise, false.</returns>
        public static bool ValidateFranceIban([NotNullWhenAttribute(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }

            var normalized = IbanHelper.NormalizeIban(iban);

            // Check length (France IBAN is exactly 27 characters)
            if (normalized.Length != FranceIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(FranceCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Bank Code (Indices 4 to 8) and Branch Code (Indices 9 to 13) -> Must be digits
            for (int i = 4; i < 14; i++)
            {
                if (!char.IsDigit(normalized[i]))
                {
                    return false;
                }
            }

            // Check RIB Key (Last 2 chars) -> Must be digits (Indices 25 to 26)
            for (int i = 25; i < 27; i++)
            {
                if (!char.IsDigit(normalized[i])) return false;
            }

            // Validate structure and checksum
            return IbanHelper.IsValidIban(normalized);
        }
        #endregion
    }
}
