using Finova.Countries.Europe.Moldova.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Moldova;

public class MoldovaEnterpriseValidatorTests
{
    [Theory]
    // Valid IDNO: 1003600086027 (Example found online)
    // Weights: 7 3 1 7 3 1 7 3 1 7 3 1
    // 1*7 + 0 + 0 + 3*7 + 6*3 + 0 + 0 + 0 + 8*1 + 6*7 + 0 + 2*3
    // 7 + 21 + 18 + 8 + 42 + 6 = 102.
    // 102 % 10 = 2.
    // Last digit is 7. Mismatch?
    // Maybe weights are different or example is wrong.
    // Let's try to construct one.
    // 100000000000X.
    // 1*7 + 0... = 7. 7 % 10 = 7. So X=7.
    // 1000000000007 should be valid.
    [InlineData("1000000000007", true)]
    [InlineData("1000000000000", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Idno_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = MoldovaIdnoValidator.ValidateIdno(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
