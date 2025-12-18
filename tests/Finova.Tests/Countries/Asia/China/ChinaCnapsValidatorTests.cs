using Finova.Core.Common;
using Finova.Countries.Asia.China.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Asia.China;

public class ChinaCnapsValidatorTests
{
    private readonly ChinaCnapsValidator _validator = new();

    [Theory]
    [InlineData("123456789012")]
    public void Validate_ValidCnaps_ReturnsSuccess(string input)
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
    [InlineData("12345678901")] // Too short
    [InlineData("1234567890123")] // Too long
    [InlineData("12345678901A")] // Non-numeric
    public void Validate_InvalidFormat_ReturnsFailure(string input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Code == ValidationErrorCode.InvalidFormat);
    }

    [Theory]
    [InlineData("102100099996", "102", "1000", "9999")]
    [InlineData("308584000013", "308", "5840", "0001")]
    public void ParseRoutingNumber_ValidInput_ReturnsDetails(string input, string expectedBank, string expectedRegion, string expectedBranch)
    {
        // Act
        var result = _validator.ParseRoutingNumber(input);

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("CN");
        result.BankCode.Should().Be(expectedBank);
        result.DistrictCode.Should().Be(expectedRegion);
        result.BranchCode.Should().Be(expectedBranch);
    }
}
