using Finova.Countries.Europe.Finland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Finland;

public class FinlandEnterpriseValidatorTests
{
    [Theory]
    [InlineData("1234567-8", false)] // Checksum likely wrong
    [InlineData("FI1234567-8", false)]
    [InlineData("12345678", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void BusinessId_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = FinlandBusinessIdValidator.ValidateBusinessId(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void BusinessId_Validate_WithValidNumber_ReturnsTrue()
    {
        // Valid Y-tunnus: 0109862-8 (Nokia)
        // Weights: 7, 9, 10, 5, 8, 4, 2
        // 0*7 + 1*9 + 0*10 + 9*5 + 8*8 + 6*4 + 2*2
        // 0 + 9 + 0 + 45 + 64 + 24 + 4 = 146
        // 146 % 11 = 3
        // Check digit = 11 - 3 = 8.
        // Matches last digit 8.

        var result = FinlandBusinessIdValidator.ValidateBusinessId("0109862-8");
        Assert.True(result.IsValid);

        var resultNoHyphen = FinlandBusinessIdValidator.ValidateBusinessId("01098628");
        Assert.True(resultNoHyphen.IsValid);

        var resultPrefix = FinlandBusinessIdValidator.ValidateBusinessId("FI01098628");
        Assert.True(resultPrefix.IsValid);
    }
}
