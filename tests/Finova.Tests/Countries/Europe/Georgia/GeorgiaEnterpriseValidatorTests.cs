using Finova.Countries.Europe.Georgia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Georgia;

public class GeorgiaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123456789", false)] // Likely invalid
    [InlineData("GE123456789", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void TaxId_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GeorgiaTaxIdValidator.ValidateTaxId(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void TaxId_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Construct valid GE Tax ID
        // Digits: 1 0 0 0 0 0 0 0 X
        // Weights: 1 2 3 4 5 6 7 8
        // Sum = 1*1 = 1.
        // Remainder = 1 % 11 = 1.
        // Check digit = 1.
        // So 100000001 should be valid.

        var result = GeorgiaTaxIdValidator.ValidateTaxId("100000001");
        Assert.True(result.IsValid);
    }
}
