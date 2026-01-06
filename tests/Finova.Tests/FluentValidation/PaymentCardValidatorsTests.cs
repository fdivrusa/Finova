using Finova.Core.PaymentCard;
using Finova.Extensions.FluentValidation;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Finova.Tests.FluentValidation;

public class PaymentCardValidatorsTests
{
    public class TestModel
    {
        public string? CardNumber { get; set; }
        public string? Cvv { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.CardNumber)
                .MustBeValidPaymentCard()
                .When(x => x.CardNumber != null);

            RuleFor(x => x.Cvv)
                .MustBeValidCvv(x => x.CardNumber);

            RuleFor(x => x.ExpirationMonth)
                .MustBeValidExpirationDate(x => x.ExpirationYear);
        }
    }

    public class BrandedValidator : AbstractValidator<TestModel>
    {
        public BrandedValidator(PaymentCardBrand brand)
        {
            RuleFor(x => x.CardNumber)
                .MustBeValidPaymentCardForBrand(brand);
        }
    }

    private readonly TestModelValidator _validator = new();

    [Theory]
    [InlineData("4532015112830366")] // Visa
    [InlineData("5425233430109903")] // Mastercard
    [InlineData("378282246310005")] // Amex
    public void MustBeValidPaymentCard_Valid_ShouldNotHaveError(string cardNumber)
    {
        var model = new TestModel { CardNumber = cardNumber };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.CardNumber);
    }

    [Fact]
    public void MustBeValidPaymentCard_Invalid_ShouldHaveError()
    {
        var model = new TestModel { CardNumber = "4532015112830367" }; // Invalid checksum
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CardNumber);
    }

    [Theory]
    [InlineData("4532015112830366", PaymentCardBrand.Visa)]
    [InlineData("5425233430109903", PaymentCardBrand.Mastercard)]
    public void MustBeValidPaymentCardForBrand_Valid_ShouldNotHaveError(string cardNumber, PaymentCardBrand brand)
    {
        var model = new TestModel { CardNumber = cardNumber };
        var validator = new BrandedValidator(brand);
        var result = validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.CardNumber);
    }

    [Fact]
    public void MustBeValidPaymentCardForBrand_Mismatch_ShouldHaveError()
    {
        var model = new TestModel { CardNumber = "4532015112830366" }; // Visa
        var validator = new BrandedValidator(PaymentCardBrand.Mastercard);
        var result = validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CardNumber);
    }

    [Theory]
    [InlineData("4532015112830366", "123")] // Visa
    [InlineData("378282246310005", "1234")] // Amex
    public void MustBeValidCvv_Valid_ShouldNotHaveError(string cardNumber, string cvv)
    {
        var model = new TestModel { CardNumber = cardNumber, Cvv = cvv };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Cvv);
    }

    [Fact]
    public void MustBeValidCvv_Invalid_ShouldHaveError()
    {
        var model = new TestModel { CardNumber = "4532015112830366", Cvv = "1234" }; // Too long for Visa
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Cvv);
    }

    [Fact]
    public void MustBeValidExpirationDate_Valid_ShouldNotHaveError()
    {
        var future = DateTime.UtcNow.AddYears(1);
        var model = new TestModel { ExpirationMonth = future.Month, ExpirationYear = future.Year };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.ExpirationMonth);
    }

    [Fact]
    public void MustBeValidExpirationDate_Expired_ShouldHaveError()
    {
        var past = DateTime.UtcNow.AddYears(-1);
        var model = new TestModel { ExpirationMonth = past.Month, ExpirationYear = past.Year };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.ExpirationMonth);
    }
}
