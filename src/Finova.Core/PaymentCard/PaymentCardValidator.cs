using Finova.Core.Common;

namespace Finova.Core.PaymentCard;

public class PaymentCardValidator : IPaymentCardValidator
{
    #region Static Methods (High-Performance Logic)

    /// <summary>
    /// Validates the card number using the Luhn Algorithm (Mod-10).
    /// </summary>
    /// <summary>
    /// Validates the card number using the Luhn Algorithm (Mod-10).
    /// </summary>
    public static ValidationResult Validate(string? cardNumber)
    {
        if (string.IsNullOrWhiteSpace(cardNumber))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        // Check for invalid characters (allow spaces and dashes)
        foreach (char c in cardNumber)
        {
            if (!char.IsDigit(c) && !char.IsWhiteSpace(c) && c != '-')
            {
                return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCardNumberFormat);
            }
        }

        var cleanNumber = cardNumber.Replace(" ", "").Replace("-", "");

        return ChecksumHelper.ValidateLuhn(cleanNumber)
            ? ValidationResult.Success()
            : ValidationResult.Failure(ValidationErrorCode.InvalidChecksum, ValidationMessages.InvalidCardNumberLuhn);
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

        // 9. RuPay: 60, 6521, 6522
        if (clean.StartsWith("60") || clean.StartsWith("6521") || clean.StartsWith("6522"))
        {
            return PaymentCardBrand.RuPay;
        }

        // 10. Mir: 2200-2204
        if (IsMir(clean))
        {
            return PaymentCardBrand.Mir;
        }

        // 11. Verve: 506099-506198, 650002-650027
        if (IsVerve(clean))
        {
            return PaymentCardBrand.Verve;
        }

        // 12. Troy: 9792
        if (clean.StartsWith("9792"))
        {
            return PaymentCardBrand.Troy;
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
            return ValidationResult.Failure(ValidationErrorCode.InvalidInput, ValidationMessages.InputCannotBeEmpty);
        }

        if (!IsDigitsOnly(cvv))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidCvvDigits);
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
            : ValidationResult.Failure(ValidationErrorCode.InvalidLength, string.Format(ValidationMessages.InvalidCvvLength, brand));
    }

    /// <summary>
    /// Checks if the expiration date is in the future.
    /// </summary>
    public static ValidationResult ValidateExpiration(int month, int year)
    {
        if (month < 1 || month > 12)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.InvalidMonth);
        }

        var now = DateTime.UtcNow;

        // Normalize 2-digit years (e.g. 25 -> 2025)
        if (year < 100)
        {
            year += 2000;
        }

        if (year < now.Year || (year == now.Year && month < now.Month))
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CardExpired);
        }

        if (year > now.Year + 20)
        {
            return ValidationResult.Failure(ValidationErrorCode.InvalidFormat, ValidationMessages.CardYearTooFar);
        }

        return ValidationResult.Success();
    }

    #endregion

    #region Private Helpers

    private static bool IsMastercard(string number)
    {
        // Range 51-55
        int firstTwo = int.Parse(number.Substring(0, 2));
        if (firstTwo >= 51 && firstTwo <= 55) return true;

        // Range 2221-2720
        int firstFour = int.Parse(number.Substring(0, 4));
        return firstFour >= 2221 && firstFour <= 2720;
    }

    private static bool IsDiscover(string number)
    {
        if (number.StartsWith("6011") || number.StartsWith("65")) return true;

        int firstThree = int.Parse(number.Substring(0, 3));
        if (firstThree >= 644 && firstThree <= 649) return true;

        int firstSix = int.Parse(number.Substring(0, 6));
        return firstSix >= 622126 && firstSix <= 622925;
    }

    private static bool IsJcb(string number)
    {
        int firstFour = int.Parse(number.Substring(0, 4));
        return firstFour >= 3528 && firstFour <= 3589;
    }

    private static bool IsDiners(string number)
    {
        if (number.Length < 3) return false;

        int firstThree = int.Parse(number.Substring(0, 3));
        if (firstThree >= 300 && firstThree <= 305) return true;
        if (firstThree == 309) return true;

        if (number.StartsWith("36") || number.StartsWith("38") || number.StartsWith("39")) return true;

        return false;
    }

    private static bool IsMir(string number)
    {
        if (number.Length < 4) return false;
        int firstFour = int.Parse(number.Substring(0, 4));
        return firstFour >= 2200 && firstFour <= 2204;
    }

    private static bool IsVerve(string number)
    {
        if (number.Length < 6) return false;
        int firstSix = int.Parse(number.Substring(0, 6));

        if (firstSix >= 506099 && firstSix <= 506198) return true;
        if (firstSix >= 650002 && firstSix <= 650027) return true;

        return false;
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
