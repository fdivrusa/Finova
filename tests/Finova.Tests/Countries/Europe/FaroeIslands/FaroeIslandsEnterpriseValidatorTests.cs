using Finova.Countries.Europe.FaroeIslands.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.FaroeIslands;

public class FaroeIslandsEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123451", true)] // Valid V-tal (Calculated)
    [InlineData("FO123451", true)] // Valid with prefix
    [InlineData("123456", false)] // Invalid checksum
    [InlineData("12345", false)] // Invalid length
    [InlineData(null, false)] // Null
    [InlineData("", false)] // Empty
    public void Vtal_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = FaroeIslandsVtalValidator.ValidateVtal(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Vtal_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        var result = FaroeIslandsVtalValidator.ValidateVtal("123451");
        Assert.True(result.IsValid);
    }
}
