using Finova.Countries.Europe.Liechtenstein.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Liechtenstein;

public class LiechtensteinNationalIdValidatorTests
{
    private readonly LiechtensteinNationalIdValidator _validator = new();

    [Theory]
    [InlineData("7561234567897")] // Example valid (Checksum needs verification)
    // 7*1 + 5*3 + 6*1 + 1*3 + 2*1 + 3*3 + 4*1 + 5*3 + 6*1 + 7*3 + 8*1 + 9*3
    // 7 + 15 + 6 + 3 + 2 + 9 + 4 + 15 + 6 + 21 + 8 + 27 = 123.
    // 123 % 10 = 3. Check = 7.
    // So 7561234567897 is valid.
    public void Validate_ValidPeid_ReturnsSuccess(string? input)
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
    [InlineData("756123456789")] // Too short
    [InlineData("75612345678901")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("1234567890123")] // Does not start with 756
    public void Validate_InvalidPrefix_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid format.");
    }

    [Theory]
    [InlineData("7561234567890")] // Invalid checksum
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
        var result = _validator.Parse(" 756.1234.5678.97 ");
        result.Should().Be("7561234567897");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
