using Finova.Countries.Europe.Kosovo.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Kosovo;

public class KosovoEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123456789", false)] // Likely invalid
    [InlineData(null, false)]
    [InlineData("", false)]
    public void FiscalNumber_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = KosovoFiscalNumberValidator.ValidateFiscalNumber(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void FiscalNumber_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Valid Fiscal Number
        // Digits: 1 0 0 0 0 0 0 0 X
        // Weights: 9, 8, 7, 6, 5, 4, 3, 2
        // Sum = 1*9 = 9.
        // Remainder = 9 % 11 = 9.
        // CheckDigit = 9.
        // 100000009

        var result = KosovoFiscalNumberValidator.ValidateFiscalNumber("100000009");
        Assert.True(result.IsValid);
    }
}
