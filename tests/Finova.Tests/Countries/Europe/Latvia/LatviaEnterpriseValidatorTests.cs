using Finova.Countries.Europe.Latvia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Latvia;

public class LatviaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("40003000000", false)] // Example, need to verify
    [InlineData("LV40003000000", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Pvn_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = LatviaPvnValidator.ValidatePvn(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Pvn_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Valid PVN
        // Digits: 4 0 0 0 3 0 0 0 0 0 X
        // Weights: 9, 1, 4, 8, 3, 10, 2, 5, 7, 6
        // Sum = 4*9 + 3*3 = 36 + 9 = 45.
        // Remainder = 45 % 11 = 1.
        // CheckDigit = 3 - 1 = 2.
        // So 40003000002 should be valid.

        var result = LatviaPvnValidator.ValidatePvn("40003000002");
        Assert.True(result.IsValid);
    }
}
