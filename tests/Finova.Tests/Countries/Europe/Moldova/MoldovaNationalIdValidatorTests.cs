using Finova.Countries.Europe.Moldova.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Moldova;

public class MoldovaNationalIdValidatorTests
{
    private readonly MoldovaNationalIdValidator _validator = new();

    [Theory]
    // Weights: 7 3 1 7 3 1 7 3 1 7 3 1
    // 2*7 + 0*3 + 0*1 + 5*7 + 0*3 + 4*1 + 2*7 + 0*3 + 0*1 + 6*7 + 2*3 + 3*1
    // 14 + 0 + 0 + 35 + 0 + 4 + 14 + 0 + 0 + 42 + 6 + 3
    // Sum = 118.
    // 118 % 10 = 8.
    // Check digit should be 8.
    // Wait, let me re-calculate or find a valid one.
    // Let's construct a valid one.
    // 200000000000
    // 2*7 = 14. Sum = 14. Rem = 4. Check = 4.
    // So 2000000000004 should be valid.
    [InlineData("2000000000004")]
    public void Validate_ValidIdnp_ReturnsSuccess(string? input)
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
    [InlineData("200000000000")] // Too short
    [InlineData("20000000000000")] // Too long
    public void Validate_InvalidLength_ReturnsFailure(string? input)
    {
        var result = _validator.Validate(input);
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle()
            .Which.Message.Should().Be("Invalid length.");
    }

    [Theory]
    [InlineData("2000000000005")] // Invalid checksum (should be 4)
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
        var result = _validator.Parse(" 2000000000004 ");
        result.Should().Be("2000000000004");
    }

    [Fact]
    public void Parse_InvalidInput_ReturnsNull()
    {
        var result = _validator.Parse("invalid");
        result.Should().BeNull();
    }
}
