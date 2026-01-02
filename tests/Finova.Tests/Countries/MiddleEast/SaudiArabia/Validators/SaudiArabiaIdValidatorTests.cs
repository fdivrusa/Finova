using Finova.Countries.MiddleEast.SaudiArabia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.MiddleEast.SaudiArabia.Validators;

public class SaudiArabiaIdValidatorTests
{
    [Theory]
    [InlineData("1000000008")] // Valid National ID (Luhn valid)
    // 100000000 -> 1*2=2. Sum=2. Check=8? 2+8=10. Yes.
    [InlineData("2000000006")] // Valid Iqama (Luhn valid)
    // 200000000 -> 2*2=4. Sum=4. Check=6? 4+6=10. Yes.
    // Wait, 2000000005: 2*1, 0*2...
    // Let's use 1000000008.
    public void Validate_WithValidId_ReturnsSuccess(string id)
    {
        var result = SaudiArabiaIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("1000000000")] // Invalid checksum
    [InlineData("3000000008")] // Invalid start (3)
    [InlineData(null)]
    [InlineData("")]
    public void Validate_WithInvalidId_ReturnsFailure(string? id)
    {
        var result = SaudiArabiaIdValidator.ValidateStatic(id);
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void Parse_WithValidId_ReturnsCleanId()
    {
        var result = new SaudiArabiaIdValidator().Parse(" 1000000008 ");
        result.Should().Be("1000000008");
    }
}
