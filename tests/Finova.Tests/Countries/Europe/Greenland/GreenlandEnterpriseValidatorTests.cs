using Finova.Countries.Europe.Greenland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Greenland;

public class GreenlandEnterpriseValidatorTests
{
    [Theory]
    [InlineData("12345678", false)] // Likely invalid
    [InlineData("GL12345678", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Cvr_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GreenlandCvrValidator.ValidateCvr(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Cvr_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Valid CVR (Calculated with weights 2, 7, 6, 5, 4, 3, 2, 1)
        // 1000000X
        // 1*2 + ... + X*1
        // 2 + X = 11. X = 9.
        // 10000009

        var result = GreenlandCvrValidator.ValidateCvr("10000009");
        Assert.True(result.IsValid);
    }
}
