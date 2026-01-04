using Finova.Core.Common;
using Finova.Countries.Asia.Japan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.Japan;

public class JapanZenginCodeValidatorTests
{
    private readonly JapanZenginCodeValidator _validator = new();

    [Theory]
    [InlineData("0001")] // Bank code only (4 digits)
    [InlineData("001")] // Branch code only (3 digits)
    [InlineData("0001001")] // Combined bank + branch (7 digits)
    [InlineData("1234567")] // Another combined format
    public void Validate_ValidZenginCode_ReturnsSuccess(string input)
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
    [InlineData("01")] // Too short (2 digits)
    [InlineData("00010")] // Invalid length (5 digits)
    [InlineData("000100")] // Invalid length (6 digits)
    [InlineData("00010011")] // Too long (8 digits)
    public void Validate_InvalidLength_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
    }

    [Theory]
    [InlineData("000A")] // Contains letter
    [InlineData("AB01")] // Contains letters
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Fact]
    public void ParseRoutingNumber_CombinedFormat_ReturnsDetails()
    {
        // Arrange
        var zenginCode = "0001001";

        // Act
        var result = _validator.ParseRoutingNumber(zenginCode);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("JP");
        result.BankCode.Should().Be("0001");
        result.BranchCode.Should().Be("001");
    }

    [Fact]
    public void ParseRoutingNumber_BankCodeOnly_ReturnsDetails()
    {
        // Arrange
        var zenginCode = "0001";

        // Act
        var result = _validator.ParseRoutingNumber(zenginCode);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("JP");
        result.BankCode.Should().Be("0001");
        result.BranchCode.Should().BeNull();
    }

    [Fact]
    public void ParseRoutingNumber_BranchCodeOnly_ReturnsDetails()
    {
        // Arrange
        var zenginCode = "001";

        // Act
        var result = _validator.ParseRoutingNumber(zenginCode);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("JP");
        result.BankCode.Should().BeNull();
        result.BranchCode.Should().Be("001");
    }

    [Fact]
    public void Parse_ValidZenginCode_ReturnsNormalizedValue()
    {
        // Arrange
        var zenginCode = "0001-001";

        // Act
        var result = _validator.Parse(zenginCode);

        // Assert
        result.Should().Be("0001001");
    }

    [Fact]
    public void Parse_InvalidZenginCode_ReturnsNull()
    {
        // Arrange
        var zenginCode = "invalid";

        // Act
        var result = _validator.Parse(zenginCode);

        // Assert
        result.Should().BeNull();
    }
}
