using Finova.Countries.Europe.Belarus.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belarus;

public class BelarusNationalIdValidatorTests
{
    private readonly BelarusNationalIdValidator _validator = new();

    [Theory]
    [InlineData("3100580A012PB5")] // 1980-05-10, Male, Minsk
    [InlineData("4100580A012PB5")] // 1980-05-10, Female, Minsk
    [InlineData("5010100A001PB1")] // 2000-01-01, Male, Minsk
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
    [InlineData("3100580A012PB")] // Too short
    [InlineData("3100580A012PB55")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("7100580A012PB5")] // Invalid century code (7)
    [InlineData("3100580Z012PB5")] // Invalid region (Z)
    [InlineData("3100580A012125")] // Invalid letters (12)
    public void Validate_InvalidFormat_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid format.");
    }

    [Theory]
    [InlineData("3320580A012PB5")] // Invalid day (32)
    [InlineData("3101380A012PB5")] // Invalid month (13)
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
        var result = _validator.Parse(" 3100580A012PB5 ");
        result.Should().Be("3100580A012PB5");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
