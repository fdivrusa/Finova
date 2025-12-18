using Finova.Countries.Europe.Gibraltar.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Gibraltar;

public class GibraltarNationalIdValidatorTests
{
    private readonly GibraltarNationalIdValidator _validator = new();

    [Theory]
    [InlineData("1234567")] // Example
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
    [InlineData("123")] // Too short
    [InlineData("1234567890123")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" 1234567 ");
        result.Should().Be("1234567");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("123");
        result.Should().BeNull();
    }
}
