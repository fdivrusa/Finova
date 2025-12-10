using Finova.Core.Common;

namespace Finova.Core.PaymentCard;

public class PaymentCardValidator : IPaymentCardValidator
{
    #region Static Methods (High-Performance Logic)

    /// <summary>
    /// Validates the card number using the Luhn Algorithm (Mod-10).
    /// </summary>
    public static ValidationResult Validate(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "Card number cannot be empty.");
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
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Card number contains invalid characters.");
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

        return (sum % 10 == 0)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, "Invalid card number (Luhn check failed).");
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
    public static ValidationResult ValidateCvv(string? cvv, PaymentCardBrand brand)
    {
        if (string.IsNullOrWhiteSpace(cvv))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, "CVV cannot be empty.");
        }

        if (!IsDigitsOnly(cvv))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "CVV must contain only digits.");
        }

        bool isValidLength = brand switch
        {
            PaymentCardBrand.AmericanExpress => cvv.Length == 4,
            PaymentCardBrand.Visa or
            PaymentCardBrand.Mastercard or
            PaymentCardBrand.Discover or
            PaymentCardBrand.JCB or
            PaymentCardBrand.DinersClub => cvv.Length == 3,
            PaymentCardBrand.Maestro => cvv.Length == 3 || cvv.Length == 0,
            _ => cvv.Length == 3 || cvv.Length == 4
        };

        return isValidLength
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidLength, $"Invalid CVV length for {brand}.");
    }

    /// <summary>
    /// Checks if the expiration date is in the future.
    /// </summary>
    public static ValidationResult ValidateExpiration(int month, int year)
    {
        if (month < 1 || month > 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Invalid month.");
        }

        var now = DateTime.UtcNow;

        // Normalize 2-digit years (e.g. 25 -> 2025)
        if (year < 100)
        {
            year += 2000;
        }

        if (year < now.Year || (year == now.Year && month < now.Month))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Card has expired.");
        }

        if (year > now.Year + 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, "Expiration year is too far in the future.");
        }

        return ValidationResult.Success();
    }

    #endregion

    #region Private Helpers

    private static bool IsMastercard(string s)
    {
        int firstTwo = int.Parse(s[..2]);
        if (firstTwo >= 51 && firstTwo <= 55)
        {
            return true;
        }

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

    ValidationResult IValidator<PaymentCardDetails>.Validate(string? cardNumber)
    {
        return Validate(cardNumber);
    }

    public PaymentCardDetails? Parse(string? cardNumber)
    {
        var result = Validate(cardNumber);
        if (!result.IsValid)
        {
            return null;
        }

        var brand = GetBrand(cardNumber);
        var clean = cardNumber!.Replace(" ", "").Replace("-", "");

        return new PaymentCardDetails
        {
            CardNumber = clean,
            Brand = brand,
            IsValid = true,
            IsLuhnValid = true
        };
    }

    ValidationResult IPaymentCardValidator.ValidateLuhn(string? cardNumber) => Validate(cardNumber);
    PaymentCardBrand IPaymentCardValidator.GetBrand(string? cardNumber) => GetBrand(cardNumber);
    ValidationResult IPaymentCardValidator.ValidateCvv(string? cvv, PaymentCardBrand brand) => ValidateCvv(cvv, brand);
    ValidationResult IPaymentCardValidator.ValidateExpiration(int month, int year) => ValidateExpiration(month, year);

    #endregion
}
