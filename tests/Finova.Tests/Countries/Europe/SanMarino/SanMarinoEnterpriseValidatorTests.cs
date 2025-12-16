using Finova.Countries.Europe.SanMarino.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.SanMarino;

public class SanMarinoEnterpriseValidatorTests
{
    [Theory]
    [InlineData("12345", true)] // Valid COE
    [InlineData("SM 12345", false)] // Prefix not stripped by validator
    [InlineData("1234", false)] // Invalid length
    [InlineData("123456", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = SanMarinoCoeValidator.ValidateCoe(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new SanMarinoCoeValidator();
        var result = validator.Normalize("12345");
        Assert.Equal("12345", result);
    }
}
