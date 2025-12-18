using Finova.Countries.Europe.Azerbaijan.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Azerbaijan;

public class AzerbaijanNationalIdValidatorTests
{
    private readonly AzerbaijanNationalIdValidator _validator = new();

    [Theory]
    [InlineData("1234567")]
    [InlineData("ABC1234")]
    [InlineData("7G54321")]
    public void Validate_ValidPin_ReturnsSuccess(string? input)
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
    [InlineData("123456")] // Too short
    [InlineData("12345678")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("123456-")] // Invalid char
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
        var result = _validator.Parse(" abc1234 ");
        result.Should().Be("ABC1234");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid1");
        result.Should().BeNull();
    }
}
