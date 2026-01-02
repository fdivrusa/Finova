using Finova.Countries.SouthAmerica.Argentina.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.SouthAmerica.Argentina.Validators;

public class ArgentinaCuitValidatorTests
{
    [Theory]
    [InlineData("20-12345678-6")] // Calculated valid
    [InlineData("20123456786")] // Without hyphens
    [InlineData("30-11111111-8")] // 30111111118?
    // 3011111111: 3*5 + 0*4 + 1*(3+2+7+6+5+4+3+2) = 15 + 32 = 47.
    // 47 % 11 = 3. Check = 8. Correct.
    public void Validate_WithValidCuit_ReturnsSuccess(string cuit)
    {
        var result = ArgentinaCuitValidator.ValidateStatic(cuit);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("20-12345678-0")] // Invalid checksum
    [InlineData("20-12345678-A")] // Invalid format
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidCuit_ReturnsFailure(string? cuit)
    {
        var result = ArgentinaCuitValidator.ValidateStatic(cuit);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidCuit_ReturnsCleanCuit()
    {
        var result = new ArgentinaCuitValidator().Parse(" 20-12345678-6 ");
        result.Should().Be("20123456786");
    }
}
