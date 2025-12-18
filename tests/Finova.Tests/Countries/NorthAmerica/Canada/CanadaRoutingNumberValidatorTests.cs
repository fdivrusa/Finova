using Finova.Core.Common;
using Finova.Countries.NorthAmerica.Canada.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.NorthAmerica.Canada;

public class CanadaRoutingNumberValidatorTests
{
    private readonly CanadaRoutingNumberValidator _validator = new();

    [Theory]
    [InlineData("012345678")]
    [InlineData("000123456")]
    public void Validate_ValidRoutingNumber_ReturnsSuccess(string input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidInput);
    }

    [Theory]
    [InlineData("123456789")] // Doesn't start with 0
    [InlineData("01234567")] // Too short
    [InlineData("0123456789")] // Too long
    [InlineData("01234A678")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        // Act
        var result = _validator.Validate(input);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("012345678", "123", "45678")]
    [InlineData("000100001", "001", "00001")]
    public void ParseRoutingNumber_ValidInput_ReturnsDetails(string input, string expectedBank, string expectedBranch)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("CA");
        result.BankCode.Should().Be(expectedBank);
        result.BranchCode.Should().Be(expectedBranch);
    }
}
