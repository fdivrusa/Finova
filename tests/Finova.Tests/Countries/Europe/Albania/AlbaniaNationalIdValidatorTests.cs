using Finova.Countries.Europe.Albania.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Albania;

public class AlbaniaNationalIdValidatorTests
{
    private readonly AlbaniaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("J90050501A")] // 1990-05-05, Male
    [InlineData("K00050501A")] // 2000-05-05, Male
    [InlineData("J90550501A")] // 1990-05-05, Female (55-50=05)
    public void Validate_ValidNid_ReturnsSuccess(string? input)
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
    [InlineData("J90505001")] // Too short
    [InlineData("J90505001AA")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("190505001A")] // Starts with digit
    [InlineData("J905050011")] // Ends with digit
    public void Validate_InvalidFormat_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Invalid format.");
    }

    [Theory]
    [InlineData("J90133001A")] // Invalid month (13)
    [InlineData("J90500001A")] // Invalid day (00)
    public void Validate_InvalidDate_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().Which.Message.Should().Be("Invalid format.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" J90050501A ");
        result.Should().Be("J90050501A");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
