using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.BosniaAndHerzegovina;

public class BosniaAndHerzegovinaNationalIdValidatorTests
{
    private readonly BosniaAndHerzegovinaNationalIdValidator _validator = new();

    [Theory]
    [InlineData("0101006500006")] // Valid JMBG (Example)
    [InlineData("0707970100004")] // Valid JMBG
    public void Validate_ValidJmbg_ReturnsSuccess(string? input)
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
    [InlineData("12345678901234")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("0101006500007")] // Invalid checksum
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
        var result = _validator.Parse(" 0101006500006 ");
        result.Should().Be("0101006500006");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
