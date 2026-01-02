using Finova.Countries.SoutheastAsia.Thailand.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SoutheastAsia.Thailand.Validators;

public class ThailandIdValidatorTests
{
    [Theory]
    [InlineData("1234567890121")] // Calculated valid: Sum=... let's verify
    // 1*13 + 2*12 + 3*11 + 4*10 + 5*9 + 6*8 + 7*7 + 8*6 + 9*5 + 0*4 + 1*3 + 2*2
    // 13 + 24 + 33 + 40 + 45 + 48 + 49 + 48 + 45 + 0 + 3 + 4 = 352
    // 352 % 11 = 0.
    // 11 - 0 = 11 -> 1.
    // So check digit should be 1.
    // "1234567890121"
    [InlineData("1-2345-67890-12-1")] // With hyphens
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = ThailandIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1234567890120")] // Invalid checksum
    [InlineData("123456789012A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = ThailandIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new ThailandIdValidator().Parse(" 1-2345-67890-12-1 ");
        result.Should().Be("1234567890121");
    }
}
