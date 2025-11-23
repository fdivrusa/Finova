using Finova.Core.Internals;
using System.Text;
using System.Text.RegularExpressions;
using System.Diagnostics.CodeAnalysis;

namespace Finova.Core.Accounts
{
    /// <summary>
    /// Helper class for IBAN validation, normalization, and formatting.
    /// Supports ISO 13616 IBAN structure and checksum validation.
    /// </summary>
    public static partial class IbanHelper
    {
        [GeneratedRegex(@"[^A-Z0-9]")]
        private static partial Regex NonAlphanumericRegex();

        private const int MinIbanLength = 15;
        private const int MaxIbanLength = 34;

        /// <summary>
        /// Validates an IBAN (structure and checksum).
        /// </summary>
        /// <param name="iban">The IBAN to validate</param>
        /// <returns>True if valid, false otherwise</returns>
        public static bool IsValidIban([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return false;
            }

            var normalized = NormalizeIban(iban);

            // Check length
            if (normalized.Length < MinIbanLength || normalized.Length > MaxIbanLength)
            {
                return false;
            }

            // Check format: 2 letters (country) + 2 digits (check) + alphanumeric
            if (!char.IsLetter(normalized[0]) || !char.IsLetter(normalized[1]))
            {
                return false;
            }

            if (!char.IsDigit(normalized[2]) || !char.IsDigit(normalized[3]))
            {
                return false;
            }

            // Validate checksum
            return ValidateChecksum(normalized);
        }

        /// <summary>
        /// Normalizes an IBAN by removing spaces and converting to uppercase.
        /// </summary>
        /// <param name="iban">The IBAN to normalize</param>
        /// <returns>Normalized IBAN</returns>
        public static string NormalizeIban(string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
            {
                return string.Empty;
            }

            return NonAlphanumericRegex().Replace(iban.ToUpperInvariant(), "");
        }

        /// <summary>
        /// Formats an IBAN with spaces every 4 characters for display.
        /// </summary>
        /// <param name="iban">The IBAN to format</param>
        /// <returns>Formatted IBAN (e.g., "BE68 5390 0754 7034")</returns>
        public static string FormatIban(string? iban)
        {
            var normalized = NormalizeIban(iban);
            if (string.IsNullOrEmpty(normalized))
            {
                return string.Empty;
            }

            var sb = new StringBuilder();
            for (int i = 0; i < normalized.Length; i++)
            {
                if (i > 0 && i % 4 == 0)
                {
                    sb.Append(' ');
                }
                sb.Append(normalized[i]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the country code from an IBAN.
        /// </summary>
        /// <param name="iban">The IBAN</param>
        /// <returns>Two-letter country code or empty string if invalid</returns>
        public static string GetCountryCode(string? iban)
        {
            var normalized = NormalizeIban(iban);
            if (normalized.Length < 2)
            {
                return string.Empty;
            }

            return normalized[..2];
        }

        /// <summary>
        /// Gets the check digits from an IBAN.
        /// </summary>
        /// <param name="iban">The IBAN</param>
        /// <returns>Check digits (0-97) or 0 if invalid</returns>
        public static int GetCheckDigits(string? iban)
        {
            var normalized = NormalizeIban(iban);
            if (normalized.Length < 4)
            {
                return 0;
            }

            if (int.TryParse(normalized.AsSpan(2, 2), out var checkDigits))
            {
                return checkDigits;
            }

            return 0;
        }

        /// <summary>
        /// Validates the IBAN checksum using modulo 97 (ISO 7064).
        /// </summary>
        /// <param name="iban">The normalized IBAN</param>
        /// <returns>True if checksum is valid, false otherwise</returns>
        public static bool ValidateChecksum([NotNullWhen(true)] string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban) || iban.Length < 4)
            {
                return false;
            }

            try
            {
                // Move first 4 characters to end: BExx... becomes ...BExx
                var rearranged = string.Concat(iban.AsSpan(4), iban.AsSpan()[..4]);

                // Convert letters to numbers: A=10, B=11, ..., Z=35
                var numericString = ConvertLettersToDigits(rearranged);

                // Calculate modulo 97 - should be 1 for valid IBAN
                var mod = Modulo97Helper.Calculate(numericString);

                return mod == 1;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Converts letters in a string to their numeric equivalents (A=10, B=11, ..., Z=35).
        /// </summary>
        private static string ConvertLettersToDigits(string input)
        {
            var sb = new StringBuilder(input.Length * 2);

            foreach (char c in input)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
                else if (char.IsLetter(c))
                {
                    // A=10, B=11, ..., Z=35
                    int value = char.ToUpperInvariant(c) - 'A' + 10;
                    sb.Append(value);
                }
                else
                {
                    throw new ArgumentException($"Invalid character in IBAN: {c}");
                }
            }

            return sb.ToString();
        }
    }
}
