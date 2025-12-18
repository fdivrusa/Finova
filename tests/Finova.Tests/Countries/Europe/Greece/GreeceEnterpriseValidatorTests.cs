using Finova.Countries.Europe.Greece.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Greece;

public class GreeceEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123456789", false)] // Likely invalid
    [InlineData("GR123456789", false)]
    [InlineData("EL123456789", false)]
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Afm_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GreeceAfmValidator.ValidateAfm(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Afm_Validate_WithValidNumber_ReturnsTrue()
    {
        // Valid AFM: 094014201 (Ministry of Finance)
        // Weights: 256, 128, 64, 32, 16, 8, 4, 2
        // Digits: 0 9 4 0 1 4 2 0 1
        // 0*256 + 9*128 + 4*64 + 0*32 + 1*16 + 4*8 + 2*4 + 0*2
        // 0 + 1152 + 256 + 0 + 16 + 32 + 8 + 0 = 1464
        // 1464 % 11 = 1
        // Check digit = 1 % 10 = 1.
        // Matches last digit 1.

        var result = GreeceAfmValidator.ValidateAfm("094014201");
        Assert.True(result.IsValid);

        var resultPrefix = GreeceAfmValidator.ValidateAfm("EL094014201");
        Assert.True(resultPrefix.IsValid);
    }
}
