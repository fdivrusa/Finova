using Finova.Countries.Europe.Netherlands.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Netherlands;

public class NetherlandsNationalIdValidatorTests
{
    private readonly NetherlandsNationalIdValidator _validator = new();

    [Theory]
    [InlineData("111222333")] // Valid BSN example
    // 1*9 + 1*8 + 1*7 + 2*6 + 2*5 + 2*4 + 3*3 + 3*2 + 3*-1
    // 9 + 8 + 7 + 12 + 10 + 8 + 9 + 6 - 3
    // 24 + 30 + 12 = 66.
    // 66 % 11 = 0. Valid.
    public void Validate_ValidBsn_ReturnsSuccess(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Validate_NullOrEmpty_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Input cannot be empty.");
    }

    [Theory]
    [InlineData("1234567")] // Too short
    [InlineData("1234567890")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("111222334")] // Invalid checksum
    public void Validate_InvalidChecksum_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid checksum.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" 111.222.333 ");
        result.Should().Be("111222333");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
