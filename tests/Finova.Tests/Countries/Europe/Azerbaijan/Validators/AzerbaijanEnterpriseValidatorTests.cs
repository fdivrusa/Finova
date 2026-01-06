
using Finova.Countries.Europe.Azerbaijan.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Azerbaijan.Validators;

public class AzerbaijanEnterpriseValidatorTests
{
    [Theory]
    [InlineData("1234567890")] // Valid VOEN
    [InlineData("AZ1234567890")] // Valid VOEN (Prefix)
    public void Validate_ValidVoen_ReturnsSuccess(string? voen)
    {
        var result = AzerbaijanVoenValidator.ValidateVoen(voen);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("0000000000")] // Invalid (All same digits)
    [InlineData("1111111111")] // Invalid (All same digits)
    [InlineData("123456789")] // Too short
    [InlineData("12345678901")] // Too long
    [InlineData("ABC1234567")] // Invalid chars
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void Validate_InvalidVoen_ReturnsFailure(string? voen)
    {
        var result = AzerbaijanVoenValidator.ValidateVoen(voen);
        Assert.False(result.IsValid);
    }

    [Fact]
    public void Voen_Format_ReturnsNormalizedString()
    {
        Assert.Equal("1234567890", AzerbaijanVoenValidator.Format("AZ 1234567890"));
    }
}
