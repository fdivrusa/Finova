using BankingHelper.Core.Interfaces;
using BankingHelper.Core.Internals;
using BankingHelper.Core.Models;
using System.Text.RegularExpressions;

namespace BankingHelper.Belgium.Services
{
    public partial class BelgianPaymentService : IPaymentReferenceGenerator
    {
        public string CountryCode => "BE";

        [GeneratedRegex(@"[^\d]")]
        private static partial Regex DigitsOnlyRegex();

        private const int OgmTotalLength = 12;

        public string Generate(string rawReference, PaymentReferenceFormat format = PaymentReferenceFormat.Domestic)
        {
            return format switch
            {
                // Domestic (OGM/VCS) - Specific to Belgium
                PaymentReferenceFormat.Domestic => GenerateOgm(rawReference),

                // ISO RF - Uses the international standard logic from Core
                PaymentReferenceFormat.IsoRf => IsoReferenceHelper.Generate(rawReference),

                _ => throw new NotSupportedException($"Format {format} is not supported by {CountryCode}")
            };
        }

        public bool IsValid(string communication)
        {
            if (string.IsNullOrWhiteSpace(communication)) return false;

            // 1. Quick check for ISO RF format (Starts with 'RF')
            if (communication.Trim().StartsWith("RF", StringComparison.OrdinalIgnoreCase))
            {
                // Delegate validation to a shared ISO validator in Core
                return IsoReferenceValidator.IsValid(communication);
            }

            // 2. Assume Domestic OGM/VCS if it doesn't match the international standard
            return ValidateOgm(communication);
        }

        // --- Private OGM/VCS Logic ---

        private static string GenerateOgm(string rawReference)
        {
            var cleanRef = DigitsOnlyRegex().Replace(rawReference, "");
            if (cleanRef.Length > 10)
                throw new ArgumentException("OGM reference data cannot exceed 10 digits.");

            // Pad the reference to 10 digits
            var paddedRef = cleanRef.PadLeft(10, '0');

            // Calculate Modulo 97 on the 10 data digits
            var mod = Modulo97Helper.Calculate(paddedRef);

            // OGM rule: Check digit is 97 if remainder is 0, otherwise the remainder itself.
            var checkDigitValue = mod == 0 ? 97 : mod;
            var checkDigit = checkDigitValue.ToString("00");

            var fullNumber = $"{paddedRef}{checkDigit}";

            // Format: +++XXX/XXXX/XXXXX+++ (12 digits total)
            return $"+++{fullNumber.Substring(0, 3)}/{fullNumber.Substring(3, 4)}/{fullNumber.Substring(7, 5)}+++";
        }

        private static bool ValidateOgm(string communication)
        {
            var digits = DigitsOnlyRegex().Replace(communication, "");

            if (digits.Length != OgmTotalLength) return false;

            var data = digits.Substring(0, 10);
            var checkDigitStr = digits.Substring(10, 2);

            // Calculate expected check digit
            var calculatedMod = Modulo97Helper.Calculate(data);
            var expectedCheckDigit = calculatedMod == 0 ? 97 : calculatedMod;

            if (int.TryParse(checkDigitStr, out var actualCheckDigit))
            {
                return expectedCheckDigit == actualCheckDigit;
            }

            return false;
        }
    }
}
