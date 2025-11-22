using System.Text;

namespace Finova.Core.Internals
{
    public static class IsoReferenceValidator
    {
        private const string IsoPrefix = "RF";

        /// <summary>
        /// Validates an ISO 11649 reference by checking the RF prefix and the Modulo 97 checksum.
        /// </summary>
        public static bool IsValid(string reference)
        {
            if (string.IsNullOrWhiteSpace(reference))
            {
                return false;
            }

            // 1. Clean and normalize (remove spaces, uppercase)
            var cleanReference = reference.Replace(" ", "").ToUpperInvariant();

            // 2. Check prefix and minimum length
            if (!cleanReference.StartsWith(IsoPrefix) || cleanReference.Length < 6) // Minimum RFxx + 2 body chars
            {
                return false;
            }

            // 3. Rearrangement for Checksum Calculation (ISO 7064/13616):
            //    Move the first 4 characters (RFxx) to the end of the string.
            var bodyAndPrefix = string.Concat(cleanReference.AsSpan(4), cleanReference.AsSpan(0, 4));

            // 4. Convert letters to numbers (A=10, B=11, ..., Z=35)
            var numericReference = ConvertLettersToDigits(bodyAndPrefix);

            // 5. Calculate Modulo 97
            // The standard requires the Modulo 97 result to be exactly 1 for validity.
            var modResult = Modulo97Helper.Calculate(numericReference);

            return modResult == 1;
        }

        /// <summary>
        /// Converts letters in the string to their numeric equivalent for calculation (A=10, Z=35).
        /// </summary>
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
                    // Converts character ASCII value to 10-35 range
                    sb.Append(c - 55);
                }
                else
                {
                    // Ignore other characters (should have been cleaned already, but safer)
                }
            }
            return sb.ToString();
        }
    }
}
