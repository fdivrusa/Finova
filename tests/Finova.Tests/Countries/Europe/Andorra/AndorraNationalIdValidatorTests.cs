using Finova.Countries.Europe.Andorra.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Andorra;

public class AndorraNationalIdValidatorTests
{
    private readonly AndorraNationalIdValidator _validator = new();

    [Theory]
    [InlineData("F123456A")] // Valid format
    [InlineData("A000000Z")] // Valid format
    public void Validate_ValidNia_ReturnsSuccess(string? input)
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
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Input cannot be empty.");
    }

    [Theory]
    [InlineData("F12345A")] // Too short
    [InlineData("F1234567A")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("1123456A")] // Starts with digit
    [InlineData("F1234561")] // Ends with digit
    public void Validate_InvalidFormat_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Invalid format.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" F-123456-A ");
        result.Should().Be("F123456A");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
