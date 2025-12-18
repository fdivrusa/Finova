using Finova.Countries.Europe.Greenland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Greenland;

public class GreenlandNationalIdValidatorTests
{
    private readonly GreenlandNationalIdValidator _validator = new();

    [Theory]
    [InlineData("0101501234")] // Example
    public void Validate_ValidCpr_ReturnsSuccess(string? input)
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
    [InlineData("123456789")] // Too short
    [InlineData("12345678901")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("3201501234")] // Invalid day
    public void Validate_InvalidDate_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid format.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" 0101501234 ");
        result.Should().Be("0101501234");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
