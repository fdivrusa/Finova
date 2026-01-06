
using Finova.Countries.Europe.Albania.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Albania.Validators;

public class AlbaniaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("K12345678L", true)] // Valid NIPT
    [InlineData("ALK12345678L", true)] // Valid NIPT (Prefix)
    [InlineData("K12345678", false)] // Too short
    [InlineData("1234567890", false)] // No letters
    [InlineData("K123456789", false)] // Invalid Format
    [InlineData(null, false)]
    [InlineData("", false)]
    [InlineData(" ", false)]
    public void Validate_ReturnsExpectedResult(string? nipt, bool expectedIsValid)
    {
        var result = AlbaniaNiptValidator.ValidateNipt(nipt);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Nipt_Format_ReturnsNormalizedString()
    {
        Assert.Equal("K12345678L", AlbaniaNiptValidator.Format("AL K12345678L"));
    }
}
