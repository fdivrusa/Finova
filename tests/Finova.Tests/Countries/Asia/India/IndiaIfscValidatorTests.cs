using Finova.Core.Common;
using Finova.Countries.Asia.India.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.India;

public class IndiaIfscValidatorTests
{
    private readonly IndiaIfscValidator _validator = new();

    [Theory]
    [InlineData("SBIN0001234")]
    [InlineData("HDFC0001234")]
    public void Validate_ValidIfsc_ReturnsSuccess(string input)
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
    [InlineData("SBIN1001234")] // 5th char not 0
    [InlineData("SBI00001234")] // Too short
    [InlineData("SBIN00012345")] // Too long
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("SBIN0001234", "SBIN", "001234")]
    [InlineData("HDFC0005678", "HDFC", "005678")]
    public void ParseRoutingNumber_ValidInput_ReturnsDetails(string input, string expectedBank, string expectedBranch)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("IN");
        result.BankCode.Should().Be(expectedBank);
        result.BranchCode.Should().Be(expectedBranch);
    }
}
