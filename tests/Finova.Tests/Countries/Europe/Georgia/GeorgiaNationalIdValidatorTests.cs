using Finova.Countries.Europe.Georgia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Georgia;

public class GeorgiaNationalIdValidatorTests
{
    private readonly GeorgiaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("01024003005")] // 11 digits
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
    [InlineData("1234567890")] // 10 digits
    [InlineData("123456789012")] // 12 digits
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("0102400300A")] // Contains letter
    public void Validate_InvalidFormat_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Input must contain only digits.");
    }

    [Fact]
    public void Parse_ValidInput_ReturnsSanitized()
    {
        var result = _validator.Parse(" 01024003005 ");
        result.Should().Be("01024003005");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
