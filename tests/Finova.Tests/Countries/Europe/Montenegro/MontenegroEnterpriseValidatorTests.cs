using Finova.Countries.Europe.Montenegro.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Montenegro;

public class MontenegroEnterpriseValidatorTests
{
    [Theory]
    // Valid PIB: 02000002 (Example)
    // Weights: 7 6 5 4 3 2 1
    // 0*7 + 2*6 + 0 + 0 + 0 + 0 + 0*1 = 12.
    // 12 % 11 = 1.
    // If remainder 1, invalid.
    // So 02000002 is invalid by this logic.

    // Let's construct a valid one.
    // 1000000X.
    // 1*7 = 7. 7 % 11 = 7.
    // CheckDigit = 11 - 7 = 4.
    // So 10000004 should be valid.
    [InlineData("10000004", true)]
    [InlineData("10000005", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Pib_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = MontenegroPibValidator.ValidatePib(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
