using Finova.Countries.Europe.Malta.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Malta;

public class MaltaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("10000001", false)] // 1*3 + ... + 1*1 = 3+1 = 4. 4%37 != 0.
    [InlineData("MT10000001", false)]
    // Valid example: 11679112 (Random valid looking number?)
    // Let's try to construct one.
    // 1000000X.
    // Weights: 3 4 6 7 8 9 10 1
    // 1*3 + X*1 = 3+X. Need (3+X)%37 == 0. X = 34. Not a digit.
    // Try 1234567X.
    // 1*3 + 2*4 + 3*6 + 4*7 + 5*8 + 6*9 + 7*10 + X*1
    // 3 + 8 + 18 + 28 + 40 + 54 + 70 + X
    // Sum = 221 + X.
    // 221 % 37 = 36.
    // Need (36 + X) % 37 == 0. X = 1.
    // So 12345671 should be valid.
    [InlineData("12345671", true)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Vat_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = MaltaVatValidator.ValidateVat(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
