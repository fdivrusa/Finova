using Finova.Countries.Europe.Norway.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Norway;

public class NorwayNationalIdValidatorTests
{
    private readonly NorwayNationalIdValidator _validator = new();

    [Theory]
    [InlineData("01010012356")] // Valid Fodselsnummer
    // Let's calculate.
    // 010100123
    // W1: 3 7 6 1 8 9 4 5 2
    // 0*3 + 1*7 + 0*6 + 1*1 + 0*8 + 0*9 + 1*4 + 2*5 + 3*2
    // 0 + 7 + 0 + 1 + 0 + 0 + 4 + 10 + 6 = 28.
    // 28 % 11 = 6.
    // C1 = 11 - 6 = 5.
    // So d10 = 5.
    // Input so far: 0101001235
    // W2: 5 4 3 2 7 6 5 4 3 2
    // 0*5 + 1*4 + 0*3 + 1*2 + 0*7 + 0*6 + 1*5 + 2*4 + 3*3 + 5*2
    // 0 + 4 + 0 + 2 + 0 + 0 + 5 + 8 + 9 + 10 = 38.
    // 38 % 11 = 5.
    // C2 = 11 - 5 = 6.
    // So d11 = 6.
    // Valid ID: 01010012356
    public void Validate_ValidFodselsnummer_ReturnsSuccess(string? input)
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
    [InlineData("0101001235")] // Too short
    [InlineData("010100123567")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("01010012357")] // Invalid checksum
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
        var result = _validator.Parse(" 010100-12356 ");
        result.Should().Be("01010012356");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
