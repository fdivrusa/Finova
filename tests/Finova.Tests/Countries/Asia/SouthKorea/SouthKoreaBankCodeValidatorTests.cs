using Finova.Core.Common;
using Finova.Countries.Asia.SouthKorea.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.SouthKorea;

public class SouthKoreaBankCodeValidatorTests
{
    private readonly SouthKoreaBankCodeValidator _validator = new();

    [Theory]
    [InlineData("004")] // KB Kookmin Bank
    [InlineData("088")] // Shinhan Bank
    [InlineData("020")] // Woori Bank
    [InlineData("081")] // Hana Bank
    [InlineData("011")] // NH Nonghyup Bank
    public void Validate_ValidBankCode_ReturnsSuccess(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("01")] // Too short
    [InlineData("0001")] // Too long
    [InlineData("A01")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("004", "004")]
    [InlineData("088", "088")]
    public void ParseRoutingNumber_ValidInput_ReturnsDetails(string input, string expectedBank)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("KR");
        result.BankCode.Should().Be(expectedBank);
    }

    [Fact]
    public void Parse_ValidBankCode_ReturnsNormalizedValue()
    {
        // Arrange
        var bankCode = "0-04";

        // Act
        var result = _validator.Parse(bankCode);

        // Assert
        result.Should().Be("004");
    }

    [Fact]
    public void Parse_InvalidBankCode_ReturnsNull()
    {
        // Arrange
        var bankCode = "invalid";

        // Act
        var result = _validator.Parse(bankCode);

        // Assert
        result.Should().BeNull();
    }
}
