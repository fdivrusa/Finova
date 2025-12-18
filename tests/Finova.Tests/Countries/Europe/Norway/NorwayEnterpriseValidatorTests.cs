using Finova.Countries.Europe.Norway.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Norway;

public class NorwayEnterpriseValidatorTests
{
    [Theory]
    [InlineData("923609016", true)] // Valid Org Number (Equinor ASA)
    [InlineData("NO 923 609 016", true)] // Valid with prefix and spaces
    [InlineData("923609017", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = NorwayOrgNumberValidator.ValidateOrgNumber(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var result = NorwayOrgNumberValidator.Normalize("NO 923 609 016");
        Assert.Equal("923609016", result);
    }
}
