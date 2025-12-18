using Finova.Extensions.FluentValidation;
using FluentValidation;
using FluentValidation.TestHelper;
using Xunit;

namespace Finova.Tests.FluentValidation;

public class BankValidatorsTests
{
    public class TestModel
    {
        public string Country { get; set; } = string.Empty;
        public string? RoutingNumber { get; set; }
        public string? AccountNumber { get; set; }
    }

    public class TestModelValidator : AbstractValidator<TestModel>
    {
        public TestModelValidator()
        {
            RuleFor(x => x.RoutingNumber)
                .MustBeValidBankRoutingNumber(x => x.Country);

            RuleFor(x => x.AccountNumber)
                .MustBeValidBankAccountNumber(x => x.Country);
        }
    }

    private readonly TestModelValidator _validator = new();

    [Theory]
    [InlineData("US", "121000248")] // Valid ABA
    [InlineData("CA", "000112345")] // Valid CA
    public void MustBeValidBankRoutingNumber_Valid_ShouldNotHaveError(string country, string routing)
    {
        var model = new TestModel { Country = country, RoutingNumber = routing };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.RoutingNumber);
    }

    [Theory]
    [InlineData("US", "123456789")] // Invalid Checksum
    [InlineData("CA", "12345")] // Invalid CA (too short)
    [InlineData("XX", "12345")] // Unsupported
    public void MustBeValidBankRoutingNumber_Invalid_ShouldHaveError(string country, string routing)
    {
        var model = new TestModel { Country = country, RoutingNumber = routing };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.RoutingNumber);
    }

    [Theory]
    [InlineData("SG", "1234567890")] // Valid SG
    [InlineData("JP", "1234567")] // Valid JP
    public void MustBeValidBankAccountNumber_Valid_ShouldNotHaveError(string country, string account)
    {
        var model = new TestModel { Country = country, AccountNumber = account };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.AccountNumber);
    }

    [Theory]
    [InlineData("SG", "123")] // Too short
    [InlineData("JP", "123456")] // Too short
    [InlineData("XX", "12345")] // Unsupported
    public void MustBeValidBankAccountNumber_Invalid_ShouldHaveError(string country, string account)
    {
        var model = new TestModel { Country = country, AccountNumber = account };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AccountNumber);
    }
}
