using Finova.Countries.Europe.Serbia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Serbia;

public class SerbiaEnterpriseValidatorTests
{
    [Theory]
    // Valid PIB calculated: 100000016
    // 10000001
    // m=10.
    // 1: m=(10+1)%10=1. m=(1*2)%11=2.
    // 0: m=(2+0)%10=2. m=(2*2)%11=4.
    // 0: m=(4+0)%10=4. m=(4*2)%11=8.
    // 0: m=(8+0)%10=8. m=(8*2)%11=16%11=5.
    // 0: m=(5+0)%10=5. m=(5*2)%11=10.
    // 0: m=(10+0)%10=0->10. m=(10*2)%11=20%11=9.
    // 0: m=(9+0)%10=9. m=(9*2)%11=18%11=7.
    // 1: m=(7+1)%10=8. m=(8*2)%11=16%11=5.
    // Check = (11-5)%10 = 6.
    // So 100000016 should be valid.
    [InlineData("100000016", true)]
    [InlineData("RS 100000016", true)] // Valid with prefix
    [InlineData("100000017", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = SerbiaPibValidator.ValidatePib(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new SerbiaPibValidator();
        var result = validator.Normalize("RS 100000016");
        Assert.Equal("100000016", result);
    }
}
