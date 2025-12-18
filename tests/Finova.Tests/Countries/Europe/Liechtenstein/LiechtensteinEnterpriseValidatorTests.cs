using Finova.Countries.Europe.Liechtenstein.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Liechtenstein;

public class LiechtensteinEnterpriseValidatorTests
{
    [Theory]
    [InlineData("CHE-123.456.789", false)] // Likely invalid checksum
    [InlineData("LI123456789", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Peid_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = LiechtensteinPeidValidator.ValidatePeid(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Peid_Validate_WithCalculatedValidNumber_ReturnsTrue()
    {
        // Valid UID (Swiss logic)
        // Digits: 1 0 0 0 0 0 0 0 X
        // Weights: 5, 4, 3, 2, 7, 6, 5, 4
        // Sum = 1*5 = 5.
        // Remainder = 5 % 11 = 5.
        // CheckDigit = (11 - 5) % 11 = 6.
        // 100000006

        var result = LiechtensteinPeidValidator.ValidatePeid("CHE-100.000.006");
        Assert.True(result.IsValid);

        var resultLi = LiechtensteinPeidValidator.ValidatePeid("LI100000006");
        Assert.True(resultLi.IsValid);
    }
}
