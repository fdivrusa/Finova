using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Germany.Validators
{
    /// <summary>
    /// Validator for Germany bank accounts.
    /// Germany IBAN format: DE + 2 check digits + 8 bank code + 10 account number (22 characters total).
    /// Example : DE89370400440532013000 or formatted: DE89 3704 0044 0532 0130 00
    /// </summary>
    public class GermanyIbanValidator : IIbanValidator
    {
        public string CountryCode => "DE";

        private const int GermanyIbanLength = 22;
        private const string GermanyCountryCode = "DE";

        #region Instance Methods (for Dependency Injection)

        public bool IsValidIban(string? iban)
        {
            return ValidateGermanyIban(iban);
        }

        #endregion

        #region Static Methods (for Direct Usage)

        /// <summary>
        /// Validates a Germany IBAN.
        /// </summary>
        /// <param name="iban">The IBAN to validate.</param>
        public static bool ValidateGermanyIban([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }
            var normalized = IbanHelper.NormalizeIban(iban);

            // Check length (Germany IBAN is exactly 22 characters)
            if (normalized.Length != GermanyIbanLength)
            {
                return false;
            }

            // Check country code
            if (!normalized.StartsWith(GermanyCountryCode, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            // Check that bank code and account number are digits
            for (int i = 2; i < 22; i++)
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