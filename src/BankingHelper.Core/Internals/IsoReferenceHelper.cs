using System.Text;

namespace BankingHelper.Core.Internals
{
    public static class IsoReferenceHelper
    {
        private const string IsoPrefix = "RF";
        private const string PlaceholderCheckDigits = "00";

        public static string Generate(string rawReference)
        {
            if (string.IsNullOrWhiteSpace(rawReference))
            {
                throw new ArgumentException("Raw reference cannot be empty for ISO generation.", nameof(rawReference));
            }

            // 1. Clean and standardize the input reference (max 25 alphanumeric chars)
            // Note: For simplicity, we just use the raw reference, but a real implementation 
            // would clean it up to only alphanumeric characters.
            var referenceBody = rawReference.Trim().ToUpperInvariant();

            // 2. Construct the string for check digit calculation:
            //    Reference Body + ISO Prefix ('RF') + Placeholder Check Digits ('00')
            var checkString = new StringBuilder()
                .Append(referenceBody)
                .Append(IsoPrefix)
                .Append(PlaceholderCheckDigits)
                .ToString();

            // 3. Convert letters to digits (A=10, B=11, ..., Z=35)
            var numericString = ConvertLettersToDigits(checkString);

            // 4. Calculate Modulo 97 (as per ISO 7064)
            // Modulo 97-10 standard requires the result to be 1.
            var mod = Modulo97Helper.Calculate(numericString);

            // 5. Calculate the two-digit check value: 98 - Modulo result
            var checkValue = 98 - mod;

            // 6. Format the final two check digits (e.g., 5 -> "05")
            var finalCheckDigits = checkValue.ToString("00");

            // 7. Final format: RFxx[Reference Body]
            return $"{IsoPrefix}{finalCheckDigits}{referenceBody}";
        }

        // Helper function moved from IbanValidationHelper (as it's a reusable ISO standard function)
        private static string ConvertLettersToDigits(string input)
        {
            var sb = new StringBuilder(input.Length * 2);
            foreach (var c in input)
            {
                if (char.IsDigit(c))
                {
                    sb.Append(c);
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // 'A' becomes 10, 'B' becomes 11, etc.
                    sb.Append(c - 55);
                }
            }
            return sb.ToString();
        }
    }
}
