using Finova.Countries.Europe.Montenegro.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Montenegro;

public class MontenegroNationalIdValidatorTests
{
    private readonly MontenegroNationalIdValidator _validator = new();

    [Theory]
    [InlineData("0101006500006")] // Generated valid JMBG
    // 010100650000
    // Weights: 7 6 5 4 3 2 7 6 5 4 3 2
    // 0*7 + 1*6 + 0*5 + 1*4 + 0*3 + 0*2 + 6*7 + 5*6 + 0*5 + 0*4 + 0*3 + 0*2
    // 0 + 6 + 0 + 4 + 0 + 0 + 42 + 30 + 0 + 0 + 0 + 0
    // Sum = 82.
    // 82 % 11 = 5.
    // Check = 11 - 5 = 6.
    // So 0101006500006 is valid.
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
    [InlineData("010100650000")] // Too short
    [InlineData("01010065000066")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("0101006500005")] // Invalid checksum (should be 6)
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
