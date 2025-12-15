using Finova.Countries.Europe.Hungary.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Hungary;

public class HungaryEnterpriseValidatorTests
{
    [Theory]
    [InlineData("12345678-1-11", false)] // Likely invalid
    [InlineData("12345678111", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Adoszam_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = HungaryAdoszamValidator.ValidateAdoszam(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Adoszam_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Valid Adoszam
        // First 8 digits: 1 0 0 0 0 0 0 X
        // Weights: 9, 7, 3, 1, 9, 7, 3
        // Sum = 1*9 = 9.
        // Remainder = 9 % 10 = 9.
        // CheckDigit = (10 - 9) % 10 = 1.
        // So 8th digit X should be 1.
        // 10000001-1-11 (Last 3 digits don't matter for checksum)

        var result = HungaryAdoszamValidator.ValidateAdoszam("10000001-1-11");
        Assert.True(result.IsValid);

        var resultClean = HungaryAdoszamValidator.ValidateAdoszam("10000001111");
        Assert.True(resultClean.IsValid);
    }
}
