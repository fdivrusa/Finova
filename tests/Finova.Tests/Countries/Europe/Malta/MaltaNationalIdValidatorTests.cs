using Finova.Countries.Europe.Malta.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Malta;

public class MaltaNationalIdValidatorTests
{
    private readonly MaltaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("1234567M")]
    [InlineData("12345G")]
    [InlineData("1A")]
    public void Validate_ValidId_ReturnsSuccess(string? input)
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
    [InlineData("12345678M")] // Too many digits
    [InlineData("M")] // No digits
    [InlineData("1234567X")] // Invalid letter
    [InlineData("1234567")] // Missing letter
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
        var result = _validator.Parse(" 1234567m ");
        result.Should().Be("1234567M");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
