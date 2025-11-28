using Finova.Core.Internals;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Belgium.Validators
{
    /// <summary>
    /// Validator for Belgian Enterprise Numbers (KBO/BCE - Kruispuntbank van Ondernemingen / Banque-Carrefour des Entreprises).
    /// Format: 0xxx.xxx.xxx or BE0xxxxxxxxx (10 digits with check digit validation via modulo 97).
    /// </summary>
    public static partial class BelgiumEnterpriseValidator
    {
        [GeneratedRegex(@"[^\d]")]
        private static partial Regex DigitsOnlyRegex();

        private const int KboLength = 10;
        private const string KboPrefix = "0";

        /// <summary>
        /// Validates a Belgian Enterprise Number (KBO/BCE).
        /// Accepts formats: 0123.456.789, BE0123456789, or 0123456789.
        /// </summary>
        /// <param name="kbo">The KBO/BCE number to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValid([NotNullWhen(true)] string? kbo)
        {
            if (string.IsNullOrWhiteSpace(kbo))
            {
                return false;
            }

            // Extract only digits
            var digits = DigitsOnlyRegex().Replace(kbo, "");

            // Must be exactly 10 digits
            if (digits.Length != KboLength)
            {
                return false;
            }

            // Must start with 0 or 1
            if (digits[0] != '0' && digits[0] != '1')
            {
                return false;
            }

            // Extract the first 8 digits (main number) and last 2 digits (check digits)
            var mainNumber = digits[..8];
            var checkDigitStr = digits.Substring(8, 2);

            if (!int.TryParse(checkDigitStr, out var providedCheckDigit))
            {
                return false;
            }

            // Calculate modulo 97 on the first 8 digits
            var mod = Modulo97Helper.Calculate(mainNumber);

            // Check digit calculation: 97 - (mainNumber % 97)
            var expectedCheckDigit = 97 - mod;

            return expectedCheckDigit == providedCheckDigit;
        }

        /// <summary>
        /// Formats a Belgian Enterprise Number in the standard format: 0xxx.xxx.xxx.
        /// </summary>
        /// <param name="kbo">The KBO/BCE number to format</param>
        /// <returns>Formatted KBO number</returns>
        /// <exception cref="ArgumentException">If the KBO number is invalid</exception>
        public static string Format(string? kbo)
        {
            if (!IsValid(kbo))
            {
                throw new ArgumentException("Invalid Belgian Enterprise Number (KBO/BCE)", nameof(kbo));
            }

            var digits = DigitsOnlyRegex().Replace(kbo, "");

            // Format as: 0xxx.xxx.xxx
            return $"{digits[0]}{digits.Substring(1, 3)}.{digits.Substring(4, 3)}.{digits.Substring(7, 3)}";
        }

        /// <summary>
        /// Normalizes a Belgian Enterprise Number by removing all non-digit characters.
        /// </summary>
        /// <param name="kbo">The KBO/BCE number to normalize</param>
        /// <returns>Normalized 10-digit KBO number</returns>
        public static string Normalize(string? kbo)
        {
            if (string.IsNullOrWhiteSpace(kbo))
            {
                return string.Empty;
            }

            return DigitsOnlyRegex().Replace(kbo, "");
        }
    }
}
