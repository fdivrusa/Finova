using Finova.Countries.Europe.Turkey.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Turkey;

public class TurkeyNationalIdValidatorTests
{
    private readonly TurkeyNationalIdValidator _validator = new();

    [Theory]
    [InlineData("10000000146")] // Valid TC Kimlik
    // 1 0 0 0 0 0 0 0 1 4 6
    // Odd: 1, 0, 0, 0, 1. Sum = 2.
    // Even: 0, 0, 0, 0. Sum = 0.
    // d10 = (2*7 - 0) % 10 = 14 % 10 = 4. Matches.
    // SumAll (1..10): 1+0+0+0+0+0+0+0+1+4 = 6.
    // d11 = 6 % 10 = 6. Matches.
    public void Validate_ValidTcKimlik_ReturnsSuccess(string? input)
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
    [InlineData("1000000014")] // Too short
    [InlineData("100000001466")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("00000000146")] // Starts with 0
    public void Validate_InvalidPrefix_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid format.");
    }

    [Theory]
    [InlineData("10000000145")] // Invalid checksum
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
        var result = _validator.Parse(" 10000000146 ");
        result.Should().Be("10000000146");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
