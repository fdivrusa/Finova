using Finova.Core.Common;
using Finova.Countries.Oceania.Australia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Oceania.Australia;

public class AustraliaBsbValidatorTests
{
    private readonly AustraliaBsbValidator _validator = new();

    [Theory]
    [InlineData("012345")]
    [InlineData("012-345")]
    public void Validate_ValidBsb_ReturnsSuccess(string input)
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
    [InlineData("12345")] // Too short
    [InlineData("1234567")] // Too long
    [InlineData("12A456")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("012345", "01", "2", "345")]
    [InlineData("732-123", "73", "2", "123")]
    public void ParseRoutingNumber_ValidInput_ReturnsDetails(string input, string expectedBank, string expectedState, string expectedBranch)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("AU");
        result.BankCode.Should().Be(expectedBank);
        result.DistrictCode.Should().Be(expectedState);
        result.BranchCode.Should().Be(expectedBranch);
    }
}
