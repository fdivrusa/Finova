
using Finova.Countries.Europe.Andorra.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.Andorra.Validators;

public class AndorraEnterpriseValidatorTests
{
    [Theory]
    [InlineData("F-123456-Z", true)] // Valid NRT
    [InlineData("F123456Z", true)] // Valid NRT (No hyphens)
    [InlineData("ADF123456Z", true)] // Valid NRT (Prefix)
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    [InlineData("12345678", false)] // No letters
    [InlineData("F12345Z", false)] // Too short
    public void Validate_ReturnsExpectedResult(string? nrt, bool expectedIsValid)
    {
        var result = AndorraNrtValidator.ValidateNrt(nrt);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Nrt_Format_ReturnsFormattedString()
    {
        Assert.Equal("F-123456-Z", AndorraNrtValidator.Format("F123456Z"));
    }
}
