
using Finova.Countries.Europe.Belarus.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belarus.Validators;

public class BelarusEnterpriseValidatorTests
{
    [Theory]
    [InlineData("100185330")] // Valid UNP
    [InlineData("BY100185330")] // Valid UNP (Prefix)
    public void Validate_ValidUnp_ReturnsSuccess(string? unp)
    {
        var result = BelarusUnpValidator.ValidateUnp(unp);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("100185338")] // Invalid Checksum
    [InlineData("12345678")] // Too short
    [InlineData("1234567890")] // Too long
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidUnp_ReturnsFailure(string? unp)
    {
        var result = BelarusUnpValidator.ValidateUnp(unp);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Unp_Format_ReturnsNormalizedString()
    {
        Assert.Equal("100185330", BelarusUnpValidator.Format("BY 100 185 330"));
    }
}
