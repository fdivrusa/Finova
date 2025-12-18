using Finova.Countries.Europe.Slovakia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Slovakia;

public class SlovakiaEnterpriseValidatorTests
{
    [Theory]
    [InlineData("2020202020", false)] // Invalid checksum
    [InlineData("1100000000", true)] // Divisible by 11
    [InlineData("SK 1100000000", true)] // Valid with prefix
    [InlineData("1100000001", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = SlovakiaVatValidator.ValidateVat(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new SlovakiaVatValidator();
        var result = validator.Normalize("SK 1100000000");
        Assert.Equal("1100000000", result);
    }
}
