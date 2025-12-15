using Finova.Countries.Europe.Lithuania.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Lithuania;

public class LithuaniaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123456789", false)] // Likely invalid
    [InlineData("LT123456789", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Pvm_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = LithuaniaPvmValidator.ValidatePvm(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Pvm_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Valid PVM
        // Digits: 1 0 0 0 0 0 0 0 X
        // Pass 1 Weights: 1, 2, 3, 4, 5, 6, 7, 8
        // Sum = 1*1 = 1.
        // Remainder = 1 % 11 = 1.
        // CheckDigit = 1.
        // 100000001

        var result = LithuaniaPvmValidator.ValidatePvm("100000001");
        Assert.True(result.IsValid);
    }
}
