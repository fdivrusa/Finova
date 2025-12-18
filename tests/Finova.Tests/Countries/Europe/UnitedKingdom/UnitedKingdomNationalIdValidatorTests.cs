using Finova.Countries.Europe.UnitedKingdom.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom;

public class UnitedKingdomNationalIdValidatorTests
{
    private readonly UnitedKingdomNationalIdValidator _validator = new();

    [Theory]
    [InlineData("QQ123456A")] // Valid format
    [InlineData("AA123456A")] // Valid format
    public void Validate_ValidNino_ReturnsSuccess(string? input)
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
    [InlineData("QQ123456")] // Too short
    [InlineData("QQ123456AB")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("GB123456A")] // Invalid prefix
    [InlineData("BG123456A")] // Invalid prefix
    [InlineData("NK123456A")] // Invalid prefix
    [InlineData("ZZ123456A")] // Invalid prefix
    [InlineData("AO123456A")] // Invalid second letter O
    public void Validate_InvalidFormat_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid format.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" QQ 12 34 56 A ");
        result.Should().Be("QQ123456A");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
