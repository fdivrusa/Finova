using Finova.Countries.Europe.Romania.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Romania;

public class RomaniaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123458", true)] // Valid CIF calculated
    [InlineData("RO 123458", true)] // Valid with prefix
    [InlineData("18547291", false)] // Invalid checksum

    // 123: Reverse 21. Weights 7, 5. Sum 14+5=19. 190%11 = 3. Check digit 3.
    [InlineData("123", true)] // Wait, is 123 valid? 12 is data, 3 is check.
    // Reverse 12 -> 21. Weights 7, 5. 2*7 + 1*5 = 19. (19*10)%11 = 190%11 = 3. Check is 3. Yes.
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = RomaniaCifValidator.ValidateCif(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new RomaniaCifValidator();
        var result = validator.Normalize("RO 18547290");
        Assert.Equal("18547290", result);
    }
}
