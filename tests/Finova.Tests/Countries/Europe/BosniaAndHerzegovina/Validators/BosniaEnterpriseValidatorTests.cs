
using Finova.Countries.Europe.BosniaAndHerzegovina.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.BosniaAndHerzegovina.Validators;

public class BosniaEnterpriseValidatorTests
{
    [Theory]
    // 420000000000 -> Weights: 7*4 + 6*2 = 28 + 12 = 40. Rem 11 = 7. Check = 4.
    // So 4200000000004 should be valid.
    [InlineData("4200000000004")]
    [InlineData("BA4200000000004")] // Valid JIB (Prefix)
    public void Validate_ValidJib_ReturnsSuccess(string? jib)
    {
        var result = BosniaJibValidator.ValidateJib(jib);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("4200000000005")] // Invalid Checksum
    [InlineData("123456789012")] // Too short
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidJib_ReturnsFailure(string? jib)
    {
        var result = BosniaJibValidator.ValidateJib(jib);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Jib_Format_ReturnsNormalizedString()
    {
        Assert.Equal("4200000000004", BosniaJibValidator.Format("BA 4200000000004"));
    }
}
