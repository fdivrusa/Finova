
using Finova.Core.PaymentCard;
using Finova.Core.Common;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Core.Validators;

public class PaymentCardValidatorTests
{
    private readonly IPaymentCardValidator _validator;

    public PaymentCardValidatorTests()
    {
        _validator = new PaymentCardValidator();
    }

    #region IsValidLuhn Tests

    [Theory]
    [InlineData("4532015112830366")] // Valid Visa
    [InlineData("5425233430109903")] // Valid Mastercard
    [InlineData("374245455400126")] // Valid Amex
    [InlineData("6011111111111117")] // Valid Discover
    [InlineData("3530111333300000")] // Valid JCB
    [InlineData("30569309025904")] // Valid Diners
    [InlineData("6304000000000000")] // Valid Maestro
    [InlineData("79927398713")] // Valid short card
    public void IsValidLuhn_WithValidCardNumbers_ReturnsTrue(string cardNumber)
    {
        PaymentCardValidator.Validate(cardNumber).IsValid.Should().BeTrue();
        ((IPaymentCardValidator)_validator).ValidateLuhn(cardNumber).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("4532 0151 1283 0366")] // Valid Visa with spaces
    [InlineData("5425-2334-3010-9903")] // Valid Mastercard with dashes
    [InlineData("3742 4545 5400 126")] // Valid Amex with spaces
    public void IsValidLuhn_WithFormattedCardNumbers_ReturnsTrue(string cardNumber)
    {
        PaymentCardValidator.Validate(cardNumber).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("4532015112830367")] // Invalid check digit
    [InlineData("5425233430109904")] // Invalid check digit
    [InlineData("374245455400127")] // Invalid check digit
    [InlineData("1234567890123456")] // Random invalid number
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    [InlineData("   ")] // Whitespace
    [InlineData("abcd1234567890")] // Contains letters
    [InlineData("4532@151#128$036")] // Contains special chars
    public void IsValidLuhn_WithInvalidCardNumbers_ReturnsFalse(string? cardNumber)
    {
        PaymentCardValidator.Validate(cardNumber).IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidLuhn_WithAllZeros_PassesLuhnButUnlikely()
    {
        // All zeros technically passes Luhn check (0 % 10 == 0)
        PaymentCardValidator.Validate("0000000000000000").IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidLuhn_WithSingleDigit_PassesLuhnCheck()
    {
        // Single digit "0" passes Luhn check (0 % 10 == 0)
        PaymentCardValidator.Validate("0").IsValid.Should().BeTrue();
    }

    #endregion

    #region GetBrand Tests - Visa

    [Theory]
    [InlineData("4532015112830366")] // 16 digit Visa
    [InlineData("4000056655665556")] // Test Visa
    [InlineData("4111111111111111")] // Classic test Visa
    [InlineData("4012888888881881")] // Another test Visa
    public void GetBrand_WithVisaCards_ReturnsVisa(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.Visa);
        ((IPaymentCardValidator)_validator).GetBrand(cardNumber).Should().Be(PaymentCardBrand.Visa);
    }

    #endregion

    #region GetBrand Tests - Mastercard

    [Theory]
    [InlineData("5425233430109903")] // Mastercard 51-55 range
    [InlineData("5105105105105100")] // Test Mastercard
    [InlineData("5555555555554444")] // Test Mastercard
    [InlineData("2221000000000009")] // New Mastercard range 2221-2720
    [InlineData("2720999999999996")] // Upper end of new range
    public void GetBrand_WithMastercardCards_ReturnsMastercard(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.Mastercard);
    }

    #endregion

    #region GetBrand Tests - American Express

    [Theory]
    [InlineData("374245455400126")] // Amex starting with 34
    [InlineData("378282246310005")] // Amex starting with 37
    [InlineData("371449635398431")] // Test Amex
    [InlineData("343434343434343")] // Test Amex
    public void GetBrand_WithAmexCards_ReturnsAmericanExpress(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.AmericanExpress);
    }

    #endregion

    #region GetBrand Tests - Discover

    [Theory]
    [InlineData("6011111111111117")] // Discover 6011
    [InlineData("6011000990139424")] // Test Discover
    [InlineData("6500000000000002")] // Discover 65
    [InlineData("6440000000000000")] // Discover 644-649
    [InlineData("6490000000000000")] // Discover 644-649
    public void GetBrand_WithDiscoverCards_ReturnsDiscover(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.Discover);
    }

    #endregion

    #region GetBrand Tests - JCB

    [Theory]
    [InlineData("3530111333300000")] // JCB lower range
    [InlineData("3566002020360505")] // JCB mid range
    [InlineData("3589000000000000")] // JCB upper range
    public void GetBrand_WithJcbCards_ReturnsJCB(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.JCB);
    }

    #endregion

    #region GetBrand Tests - Diners Club

    [Theory]
    [InlineData("30569309025904")] // Diners 300-305
    [InlineData("30000000000004")] // Diners 300
    [InlineData("30500000000003")] // Diners 305
    [InlineData("30900000000005")] // Diners 309
    [InlineData("36000000000000")] // Diners 36
    [InlineData("38000000000000")] // Diners 38
    [InlineData("39000000000000")] // Diners 39
    public void GetBrand_WithDinersCards_ReturnsDinersClub(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.DinersClub);
    }

    #endregion

    #region GetBrand Tests - China UnionPay

    [Theory]
    [InlineData("6200000000000005")] // UnionPay starting with 62
    [InlineData("6280000000000001")] // UnionPay
    [InlineData("6299999999999999")] // UnionPay
    public void GetBrand_WithUnionPayCards_ReturnsChinaUnionPay(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.ChinaUnionPay);
    }

    #endregion

    #region GetBrand Tests - Maestro

    [Theory]
    [InlineData("5000000000000000")] // Maestro 50
    [InlineData("5600000000000000")] // Maestro 56
    [InlineData("5700000000000000")] // Maestro 57
    [InlineData("5800000000000000")] // Maestro 58
    [InlineData("6300000000000000")] // Maestro 6x
    [InlineData("6700000000000000")] // Maestro 6x
    public void GetBrand_WithMaestroCards_ReturnsMaestro(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.Maestro);
    }

    #endregion

    #region GetBrand Tests - Edge Cases

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("123")] // Too short
    public void GetBrand_WithInvalidInput_ReturnsUnknown(string? cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.Unknown);
    }

    [Theory]
    [InlineData("1234567890123456")] // Doesn't match any brand
    [InlineData("9999999999999999")] // Doesn't match any brand
    public void GetBrand_WithUnrecognizedCard_ReturnsUnknown(string cardNumber)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(PaymentCardBrand.Unknown);
    }

    [Theory]
    [InlineData("4532 0151 1283 0366", PaymentCardBrand.Visa)] // With spaces
    [InlineData("5425-2334-3010-9903", PaymentCardBrand.Mastercard)] // With dashes
    public void GetBrand_WithFormattedCards_DetectsBrandCorrectly(string cardNumber, PaymentCardBrand expected)
    {
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(expected);
    }

    #endregion

    #region IsValidCvv Tests

    [Theory]
    [InlineData("123", PaymentCardBrand.Visa)]
    [InlineData("456", PaymentCardBrand.Mastercard)]
    [InlineData("789", PaymentCardBrand.Discover)]
    [InlineData("000", PaymentCardBrand.JCB)]
    [InlineData("999", PaymentCardBrand.DinersClub)]
    public void IsValidCvv_WithThreeDigitCvv_ForStandardCards_ReturnsTrue(string cvv, PaymentCardBrand brand)
    {
        PaymentCardValidator.ValidateCvv(cvv, brand).IsValid.Should().BeTrue();
        ((IPaymentCardValidator)_validator).ValidateCvv(cvv, brand).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1234")]
    [InlineData("0000")]
    [InlineData("9999")]
    public void IsValidCvv_WithFourDigitCvv_ForAmex_ReturnsTrue(string cvv)
    {
        PaymentCardValidator.ValidateCvv(cvv, PaymentCardBrand.AmericanExpress).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("12", PaymentCardBrand.Visa)] // Too short
    [InlineData("1234", PaymentCardBrand.Visa)] // Too long for Visa
    [InlineData("123", PaymentCardBrand.AmericanExpress)] // Too short for Amex
    [InlineData("12345", PaymentCardBrand.AmericanExpress)] // Too long for Amex
    public void IsValidCvv_WithWrongLength_ReturnsFalse(string cvv, PaymentCardBrand brand)
    {
        PaymentCardValidator.ValidateCvv(cvv, brand).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("12a", PaymentCardBrand.Visa)] // Contains letter
    [InlineData("1@3", PaymentCardBrand.Mastercard)] // Contains special char
    [InlineData("", PaymentCardBrand.Visa)] // Empty
    [InlineData(null, PaymentCardBrand.Visa)] // Null
    [InlineData("   ", PaymentCardBrand.Visa)] // Whitespace
    public void IsValidCvv_WithNonDigitCvv_ReturnsFalse(string? cvv, PaymentCardBrand brand)
    {
        PaymentCardValidator.ValidateCvv(cvv, brand).IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidCvv_ForMaestro_AllowsThreeDigits()
    {
        PaymentCardValidator.ValidateCvv("123", PaymentCardBrand.Maestro).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidCvv_ForMaestro_EmptyStringNotAllowed()
    {
        // Maestro actually requires CVV when present, empty string is invalid
        PaymentCardValidator.ValidateCvv("", PaymentCardBrand.Maestro).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234")]
    public void IsValidCvv_ForUnknownBrand_AcceptsThreeOrFour(string cvv)
    {
        PaymentCardValidator.ValidateCvv(cvv, PaymentCardBrand.Unknown).IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("123")]
    [InlineData("1234")]
    public void IsValidCvv_ForChinaUnionPay_AcceptsThreeOrFour(string cvv)
    {
        PaymentCardValidator.ValidateCvv(cvv, PaymentCardBrand.ChinaUnionPay).IsValid.Should().BeTrue();
    }

    #endregion

    #region IsValidExpiration Tests

    [Fact]
    public void IsValidExpiration_WithFutureDate_ReturnsTrue()
    {
        var futureYear = DateTime.UtcNow.Year + 2;
        PaymentCardValidator.ValidateExpiration(12, futureYear).IsValid.Should().BeTrue();
        ((IPaymentCardValidator)_validator).ValidateExpiration(12, futureYear).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidExpiration_WithCurrentMonthAndYear_ReturnsTrue()
    {
        var now = DateTime.UtcNow;
        PaymentCardValidator.ValidateExpiration(now.Month, now.Year).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidExpiration_WithPastYear_ReturnsFalse()
    {
        var pastYear = DateTime.UtcNow.Year - 1;
        PaymentCardValidator.ValidateExpiration(12, pastYear).IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValidExpiration_WithPastMonthCurrentYear_ReturnsFalse()
    {
        var now = DateTime.UtcNow;
        if (now.Month > 1)
        {
            PaymentCardValidator.ValidateExpiration(now.Month - 1, now.Year).IsValid.Should().BeFalse();
        }
    }

    [Theory]
    [InlineData(0)] // Invalid month
    [InlineData(-1)] // Negative month
    [InlineData(13)] // Month too high
    [InlineData(99)] // Way too high
    public void IsValidExpiration_WithInvalidMonth_ReturnsFalse(int month)
    {
        var futureYear = DateTime.UtcNow.Year + 1;
        PaymentCardValidator.ValidateExpiration(month, futureYear).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(12, 25)] // Two digit year (should be normalized to 2025)
    [InlineData(12, 30)] // Two digit year (should be normalized to 2030)
    public void IsValidExpiration_WithTwoDigitYear_NormalizesCorrectly(int month, int year)
    {
        PaymentCardValidator.ValidateExpiration(month, year).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidExpiration_WithYearTooFarInFuture_ReturnsFalse()
    {
        var farFutureYear = DateTime.UtcNow.Year + 25; // More than 20 years
        PaymentCardValidator.ValidateExpiration(12, farFutureYear).IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData(1, 2025)]
    [InlineData(6, 2026)]
    [InlineData(12, 2030)]
    public void IsValidExpiration_WithValidDates_ReturnsTrue(int month, int year)
    {
        if (year > DateTime.UtcNow.Year || (year == DateTime.UtcNow.Year && month >= DateTime.UtcNow.Month))
        {
            PaymentCardValidator.ValidateExpiration(month, year).IsValid.Should().BeTrue();
        }
    }

    [Fact]
    public void IsValidExpiration_WithExactCurrentMonth_ReturnsTrue()
    {
        var now = DateTime.UtcNow;
        PaymentCardValidator.ValidateExpiration(now.Month, now.Year).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IsValidExpiration_AtBoundary_TwentyYearsInFuture_ReturnsTrue()
    {
        var boundaryYear = DateTime.UtcNow.Year + 20;
        PaymentCardValidator.ValidateExpiration(12, boundaryYear).IsValid.Should().BeTrue();
    }

    #endregion

    #region IPaymentCardValidator Interface Tests

    [Fact]
    public void IPaymentCardValidator_ValidateLuhn_DelegatesToStaticMethod()
    {
        var validator = (IPaymentCardValidator)_validator;
        var cardNumber = "4532015112830366";

        var staticResult = PaymentCardValidator.Validate(cardNumber);
        var instanceResult = validator.ValidateLuhn(cardNumber);

        instanceResult.Should().BeEquivalentTo(staticResult);
    }

    [Fact]
    public void IPaymentCardValidator_GetBrand_DelegatesToStaticMethod()
    {
        var validator = (IPaymentCardValidator)_validator;
        var cardNumber = "4532015112830366";

        var staticResult = PaymentCardValidator.GetBrand(cardNumber);
        var instanceResult = validator.GetBrand(cardNumber);

        instanceResult.Should().Be(staticResult);
    }

    [Fact]
    public void IPaymentCardValidator_ValidateCvv_DelegatesToStaticMethod()
    {
        var validator = (IPaymentCardValidator)_validator;
        var cvv = "123";
        var brand = PaymentCardBrand.Visa;

        var staticResult = PaymentCardValidator.ValidateCvv(cvv, brand);
        var instanceResult = validator.ValidateCvv(cvv, brand);

        instanceResult.Should().BeEquivalentTo(staticResult);
    }

    [Fact]
    public void IPaymentCardValidator_ValidateExpiration_DelegatesToStaticMethod()
    {
        var validator = (IPaymentCardValidator)_validator;
        var month = 12;
        var year = DateTime.UtcNow.Year + 1;

        var staticResult = PaymentCardValidator.ValidateExpiration(month, year);
        var instanceResult = validator.ValidateExpiration(month, year);

        instanceResult.Should().BeEquivalentTo(staticResult);
    }

    #endregion

    #region Integration Tests

    [Theory]
    [InlineData("4532015112830366", PaymentCardBrand.Visa, true)]
    [InlineData("5425233430109903", PaymentCardBrand.Mastercard, true)]
    [InlineData("374245455400126", PaymentCardBrand.AmericanExpress, true)]
    [InlineData("6011111111111117", PaymentCardBrand.Discover, true)]
    [InlineData("3530111333300000", PaymentCardBrand.JCB, true)]
    public void IntegrationTest_ValidateCompleteCard(string cardNumber, PaymentCardBrand expectedBrand, bool shouldBeValidLuhn)
    {
        // Test Luhn validation
        PaymentCardValidator.Validate(cardNumber).IsValid.Should().Be(shouldBeValidLuhn);

        // Test brand detection
        PaymentCardValidator.GetBrand(cardNumber).Should().Be(expectedBrand);

        // Test CVV validation
        var cvvLength = expectedBrand == PaymentCardBrand.AmericanExpress ? "1234" : "123";
        PaymentCardValidator.ValidateCvv(cvvLength, expectedBrand).IsValid.Should().BeTrue();

        // Test expiration
        var futureYear = DateTime.UtcNow.Year + 2;
        PaymentCardValidator.ValidateExpiration(12, futureYear).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IntegrationTest_CompleteCardValidation_Visa()
    {
        var cardNumber = "4532015112830366";
        var cvv = "123";
        var month = 12;
        var year = DateTime.UtcNow.Year + 2;

        PaymentCardValidator.Validate(cardNumber).IsValid.Should().BeTrue();
        var brand = PaymentCardValidator.GetBrand(cardNumber);
        brand.Should().Be(PaymentCardBrand.Visa);
        PaymentCardValidator.ValidateCvv(cvv, brand).IsValid.Should().BeTrue();
        PaymentCardValidator.ValidateExpiration(month, year).IsValid.Should().BeTrue();
    }

    [Fact]
    public void IntegrationTest_CompleteCardValidation_AmericanExpress()
    {
        var cardNumber = "374245455400126";
        var cvv = "1234"; // Amex uses 4 digits
        var month = 12;
        var year = DateTime.UtcNow.Year + 2;

        PaymentCardValidator.Validate(cardNumber).IsValid.Should().BeTrue();
        var brand = PaymentCardValidator.GetBrand(cardNumber);
        brand.Should().Be(PaymentCardBrand.AmericanExpress);
        PaymentCardValidator.ValidateCvv(cvv, brand).IsValid.Should().BeTrue();
        PaymentCardValidator.ValidateExpiration(month, year).IsValid.Should().BeTrue();
    }

    #region Instance Methods (Validate/Parse) Tests

    [Fact]
    public void Validate_WithValidCard_ReturnsSuccess()
    {
        var result = _validator.Validate("4532015112830366");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Validate_WithInvalidCard_ReturnsFailure()
    {
        var result = _validator.Validate("4532015112830367"); // Invalid check digit
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidChecksum);
    }

    [Fact]
    public void Validate_WithEmptyCard_ReturnsFailure()
    {
        var result = _validator.Validate("");
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Fact]
    public void Parse_WithValidCard_ReturnsDetails()
    {
        var details = _validator.Parse("4532015112830366");
        details.Should().NotBeNull();
        details!.IsValid.Should().BeTrue();
        details.IsLuhnValid.Should().BeTrue();
        details.Brand.Should().Be(PaymentCardBrand.Visa);
        details.CardNumber.Should().Be("4532015112830366");
    }

    [Fact]
    public void Parse_WithFormattedCard_ReturnsNormalizedDetails()
    {
        var details = _validator.Parse("4532 0151 1283 0366");
        details.Should().NotBeNull();
        details!.CardNumber.Should().Be("4532015112830366");
        details.Brand.Should().Be(PaymentCardBrand.Visa);
    }

    [Fact]
    public void Parse_WithInvalidCard_ReturnsNull()
    {
        var details = _validator.Parse("4532015112830367");
        details.Should().BeNull();
    }

    #endregion

    #endregion
}
