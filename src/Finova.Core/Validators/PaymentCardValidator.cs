using Finova.Core.Interfaces;
using Finova.Core.Models;

namespace Finova.Core.Validators
{
    public class PaymentCardValidator : IPaymentCardValidator
    {
        #region Static Methods (High-Performance Logic)

        /// <summary>
        /// Validates the card number using the Luhn Algorithm (Mod-10).
        /// </summary>
        public static bool IsValidLuhn(string? cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return false;
            }

            int sum = 0;
            bool alternate = false;

            // Loop from right to left, ignoring non-digits
            for (int i = cardNumber.Length - 1; i >= 0; i--)
            {
                char c = cardNumber[i];

                if (!char.IsDigit(c))
                {
                    // Allow spaces and dashes, reject other chars
                    if (char.IsWhiteSpace(c) || c == '-')
                    {
                        continue;
                    }
                    return false;
                }

                int n = c - '0';

                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }

                sum += n;
                alternate = !alternate;
            }

            return (sum % 10 == 0);
        }

        /// <summary>
        /// Detects the card brand based on IIN (Issuer Identification Number) ranges.
        /// </summary>
        public static PaymentCardBrand GetBrand(string? cardNumber)
        {
            if (string.IsNullOrWhiteSpace(cardNumber))
            {
                return PaymentCardBrand.Unknown;
            }

            // Normalize input for checking (removes spaces/dashes)
            // We use a simplified check here. For zero-allocation on very hot paths,
            // you would manually iterate the string without Replace().
            var clean = cardNumber.Replace(" ", "").Replace("-", "");

            if (clean.Length < 12)
            {
                return PaymentCardBrand.Unknown;
            }

            // 1. Visa: Starts with 4
            if (clean[0] == '4')
            {
                return PaymentCardBrand.Visa;
            }

            // 2. American Express: 34, 37
            if (clean.StartsWith("34") || clean.StartsWith("37"))
            {
                return PaymentCardBrand.AmericanExpress;
            }

            // 3. Mastercard: 51-55 or 2221-2720
            if (IsMastercard(clean))
            {
                return PaymentCardBrand.Mastercard;
            }

            // 4. Discover: 6011, 65, 644-649, 622126-622925
            if (IsDiscover(clean))
            {
                return PaymentCardBrand.Discover;
            }

            // 5. China UnionPay: 62
            if (clean.StartsWith("62"))
            {
                return PaymentCardBrand.ChinaUnionPay;
            }

            // 6. JCB: 3528-3589
            if (IsJcb(clean))
            {
                return PaymentCardBrand.JCB;
            }

            // 7. Diners Club: 300-305, 309, 36, 38-39
            if (IsDiners(clean))
            {
                return PaymentCardBrand.DinersClub;
            }

            // 8. Maestro: 50, 56-69
            if (clean.StartsWith("50") || (clean[0] == '5' && clean[1] >= '6') || clean.StartsWith("6"))
            {
                return PaymentCardBrand.Maestro;
            }

            return PaymentCardBrand.Unknown;
        }

        /// <summary>
        /// Validates the CVV length based on the card brand.
        /// </summary>
        public static bool IsValidCvv(string? cvv, PaymentCardBrand brand)
        {
            if (string.IsNullOrWhiteSpace(cvv) || !IsDigitsOnly(cvv))
            {
                return false;
            }

            return brand switch
            {
                // Amex uses 4 digits (CID) on the front
                PaymentCardBrand.AmericanExpress => cvv.Length == 4,

                // Most others use 3 digits (CVV2/CVC2) on the back
                PaymentCardBrand.Visa or
                PaymentCardBrand.Mastercard or
                PaymentCardBrand.Discover or
                PaymentCardBrand.JCB or
                PaymentCardBrand.DinersClub => cvv.Length == 3,

                // Maestro is tricky, sometimes has no CVV, but if present usually 3
                PaymentCardBrand.Maestro => cvv.Length == 3 || cvv.Length == 0,

                // Unknown/UnionPay default to checking for 3 or 4 to be safe
                _ => cvv.Length == 3 || cvv.Length == 4
            };
        }

        /// <summary>
        /// Checks if the expiration date is in the future.
        /// </summary>
        public static bool IsValidExpiration(int month, int year)
        {
            if (month < 1 || month > 12)
            {
                return false;
            }

            var now = DateTime.UtcNow;

            // Normalize 2-digit years (e.g. 25 -> 2025)
            if (year < 100)
            {
                year += 2000;
            }

            if (year < now.Year)
            {
                return false;
            }
            if (year == now.Year && month < now.Month)
            {
                return false;
            }

            // Optional: Set a max validity (e.g., 20 years) to catch typos like year 2999
            if (year > now.Year + 20)
            {
                return false;
            }

            return true;
        }

        #endregion

        #region Private Helpers

        private static bool IsMastercard(string s)
        {
            // Range 51-55
            int firstTwo = int.Parse(s[..2]);
            if (firstTwo >= 51 && firstTwo <= 55)
            {
                return true;
            }

            // Range 2221-2720
            int firstFour = int.Parse(s[..4]);
            if (firstFour >= 2221 && firstFour <= 2720)
            {
                return true;
            }

            return false;
        }

        private static bool IsDiscover(string s)
        {
            if (s.StartsWith("6011") || s.StartsWith("65"))
            {
                return true;
            }

            int firstThree = int.Parse(s[..3]);
            if (firstThree >= 644 && firstThree <= 649)
            {
                return true;
            }

            int firstSix = int.Parse(s[..6]);
            if (firstSix >= 622126 && firstSix <= 622925)
            {
                return true;
            }

            return false;
        }

        private static bool IsJcb(string s)
        {
            int firstFour = int.Parse(s[..4]);
            return firstFour >= 3528 && firstFour <= 3589;
        }

        private static bool IsDiners(string s)
        {
            if (s.StartsWith("36") || s.StartsWith("38") || s.StartsWith("39"))
            {
                return true;
            }
            int firstThree = int.Parse(s[..3]);
            return (firstThree >= 300 && firstThree <= 305) || firstThree == 309;
        }

        private static bool IsDigitsOnly(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }

        #endregion

        #region IPaymentCardValidator Implementation (DI Wrapper)

        bool IPaymentCardValidator.IsValidLuhn(string? cardNumber) => IsValidLuhn(cardNumber);
        PaymentCardBrand IPaymentCardValidator.GetBrand(string? cardNumber) => GetBrand(cardNumber);
        bool IPaymentCardValidator.IsValidCvv(string? cvv, PaymentCardBrand brand) => IsValidCvv(cvv, brand);
        bool IPaymentCardValidator.IsValidExpiration(int month, int year) => IsValidExpiration(month, year);

        #endregion
    }
}