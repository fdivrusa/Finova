using Finova.Countries.Europe.Poland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Poland;

public class PolandEnterpriseValidatorTests
{
    [Theory]
    [InlineData("5260250995", true)] // Valid NIP (Microsoft Sp. z o.o.)
    [InlineData("PL 526-025-09-95", true)] // Valid with prefix and separators
    [InlineData("5260250996", false)] // Invalid checksum
    [InlineData("123", false)] // Invalid length
    public void Validate_ShouldReturnExpectedResult(string number, bool expectedIsValid)
    {
        var result = PolandNipValidator.ValidateNip(number);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Normalize_ShouldReturnCleanedNumber()
    {
        var validator = new PolandNipValidator();
        var result = validator.Normalize("PL 526-025-09-95");
        Assert.Equal("5260250995", result);
    }
}
