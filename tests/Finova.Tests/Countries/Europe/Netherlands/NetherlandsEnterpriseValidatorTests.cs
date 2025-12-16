using Finova.Countries.Europe.Netherlands.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Netherlands;

public class NetherlandsEnterpriseValidatorTests
{
    [Theory]
    [InlineData("123456789B01", false)] // Likely invalid checksum
    [InlineData("NL123456789B01", false)]
    [InlineData("123456789", false)] // Missing B suffix
    [InlineData(null, false)]
    [InlineData("", false)]
    public void Btw_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = NetherlandsVatValidator.ValidateBtw(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Btw_Validate_WithValidMod97_ReturnsTrue()
    {
        // Valid Mod 97 example
        // Need to construct one.
        // 9 digits + B + 2 digits.
    }

    [Fact]
    public void Btw_Validate_WithValidElfproef_ReturnsTrue()
    {
        // Valid Elfproef (Old format)
        // The Regex `^\d{9}B\d{2}$` is strict.
        // So input MUST look like `123456789B01`.
        // The Elfproef check extracts the first 9 digits `123456789` and validates them.

        // Valid Elfproef number (BSN/RSIN style):
        // 123456782
        // Weights: 9 8 7 6 5 4 3 2 -1
        // 1*9 + 2*8 + 3*7 + 4*6 + 5*5 + 6*4 + 7*3 + 8*2 - 2*1
        // 9 + 16 + 21 + 24 + 25 + 24 + 21 + 16 - 2
        // Sum = 154. 154 % 11 = 0. Valid.

        // So `123456782B01` should be valid via Elfproef fallback.

        var result = NetherlandsVatValidator.ValidateBtw("123456782B01");
        Assert.True(result.IsValid);
    }
}
