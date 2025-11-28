using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Italy.Validators
{
    /// <summary>
    /// Validator for Italian bank accounts.
    /// Italy IBAN format: IT + 2 check + 1 CIN + 5 ABI + 5 CAB + 12 Account (27 characters total).
    /// </summary>
    public class ItalyIbanValidator : IIbanValidator
    {
        public string CountryCode => "IT";

        private const int ItalyIbanLength = 27;
        private const string ItalyCountryCode = "IT";

        #region Instance Methods (for Dependency Injection)

        public bool IsValidIban(string? iban)
        {
            return ValidateItalyIban(iban);
        }

        #endregion

        #region Static Methods (for Direct Usage)

        /// <summary>
        /// Validates an Italian IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate.</param>
        public static bool ValidateItalyIban([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }
            var normalized = IbanHelper.NormalizeIban(iban);

            // Check length (Italy IBAN is exactly 27 characters)
            if (normalized.Length != ItalyIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(ItalyCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Structure Validation:
            // CIN (Pos 4) must be a letter
            if (!char.IsLetter(normalized[4]))
            {
                return false;
            }

            // ABI (Pos 5-9) and CAB (Pos 10-14) must be digits
            for (int i = 5; i < 15; i++)
            {
                if (!char.IsDigit(normalized[i])) return false;
            }

            // Account Number (Pos 15-26) is usually numeric but legally alphanumeric in Italy
            for (int i = 15; i < 27; i++)
            {
                if (!char.IsLetterOrDigit(normalized[i])) return false;
            }
            return IbanHelper.IsValidIban(normalized);
        }

        #endregion
    }
}