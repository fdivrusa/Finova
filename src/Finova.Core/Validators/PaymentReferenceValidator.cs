using Finova.Core.Accounts;
using Finova.Core.Interfaces;
using Finova.Core.Models;

namespace Finova.Core.Validators
{
    public class PaymentReferenceValidator : IPaymentReferenceValidator
    {
        private const string IsoPrefix = "RF";

        #region Static Methods (High-Performance)

        /// <summary>
        /// Validates an ISO 11649 (RF) Reference.
        /// </summary>
        public static bool IsValid(string? reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return false;
            }

            // Normalize: Remove spaces, uppercase
            string clean = reference.Replace(" ", "").ToUpperInvariant();

            // 1. Length Check (Min 5: "RFxxC", Max 25)
            if (clean.Length < 5 || clean.Length > 25)
            {
                return false;
            }

            // 2. Prefix Check
            if (!clean.StartsWith(IsoPrefix))
            {
                return false;
            }

            // 3. Character Check (Alphanumeric only)
            foreach (char c in clean)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    return false;
                }
            }

            // 4. Modulo 97 Check
            // Algorithm: Move first 4 chars (RFxx) to the end, then Calc Mod 97.
            // Result must be 1.
            string rearranged = clean[4..] + clean[0..4];

            // Reusing your existing IbanHelper logic is best for performance
            return IbanHelper.CalculateMod97(rearranged) == 1;
        }

        /// <summary>
        /// Parses an RF reference.
        /// </summary>
        public static PaymentReferenceDetails? Parse(string? reference)
        {
            if (!IsValid(reference)) return null;

            string clean = reference!.Replace(" ", "").ToUpperInvariant();

            return new PaymentReferenceDetails
            {
                Reference = clean,
                Content = clean[4..], // Everything after "RFxx"
                Format = PaymentReferenceFormat.IsoRf,
                IsValid = true
            };
        }

        /// <summary>
        /// Generates a valid RF reference from raw content.
        /// </summary>
        public static string Generate(string rawContent)
        {
            if (string.IsNullOrWhiteSpace(rawContent))
            {
                throw new ArgumentException("Content cannot be empty", nameof(rawContent));
            }

            string cleanContent = rawContent.Replace(" ", "").ToUpperInvariant();

            // 1. Append "RF00" to the end for calculation
            string temp = cleanContent + "RF00";

            // 2. Calculate Mod 97
            // Note: If you don't have IbanHelper exposed, you can copy your previous ConvertLettersToDigits logic here
            int mod = IbanHelper.CalculateMod97(temp);

            // 3. Calculate Check Digits (98 - Mod)
            int checkValue = 98 - mod;
            string checkDigits = checkValue < 10 ? "0" + checkValue : checkValue.ToString();

            // 4. Return formatted
            return $"{IsoPrefix}{checkDigits}{cleanContent}";
        }

        #endregion

        #region Interface Implementation (DI Wrapper)

        bool IPaymentReferenceValidator.IsValid(string? reference) => IsValid(reference);
        PaymentReferenceDetails? IPaymentReferenceValidator.Parse(string? reference) => Parse(reference);
        string IPaymentReferenceValidator.Generate(string rawContent) => Generate(rawContent);

        #endregion
    }
}