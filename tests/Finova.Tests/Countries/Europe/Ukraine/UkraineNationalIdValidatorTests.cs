using Finova.Countries.Europe.Ukraine.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ukraine;

public class UkraineNationalIdValidatorTests
{
    private readonly UkraineNationalIdValidator _validator = new();

    [Theory]
    [InlineData("1234567899")] // Valid RNTRC
    // 1 2 3 4 5 6 7 8 9
    // W: -1 5 7 9 4 6 10 5 7
    // 1*-1 + 2*5 + 3*7 + 4*9 + 5*4 + 6*6 + 7*10 + 8*5 + 9*7
    // -1 + 10 + 21 + 36 + 20 + 36 + 70 + 40 + 63
    // Sum = 295.
    // 295 % 11 = 9.
    // Check = 9.
    // So 1234567899 is valid.
    public void Validate_ValidRntrc_ReturnsSuccess(string? input)
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
    [InlineData("123456789")] // Too short
    [InlineData("12345678901")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("1234567898")] // Invalid checksum
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
        var result = _validator.Parse(" 1234567899 ");
        result.Should().Be("1234567899");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
