using Finova.Countries.Europe.Bulgaria.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.Bulgaria.Validators;

public class BulgariaEnterpriseValidatorTests
{
    [Theory]
    // 9 Digits
    [InlineData("121020523", true)] // Valid UIC (9 digits)
    [InlineData("BG121020523", true)] // Valid UIC (Prefix)
    [InlineData("121020521", false)] // Invalid Checksum

    // 13 Digits
    // Base 121020523 is valid.
    // Suffix: 0000.
    // Weights 2,7,3,5 on 3,0,0,0 -> Sum 6. Rem 6. Check 6.
    // So 1210205230006 should be valid.

    [InlineData("1210205230006", true)] // Valid UIC (13 digits)
    [InlineData("1210205230001", false)] // Invalid Checksum (13th digit)
    [InlineData("1210205210000", false)] // Invalid Base UIC

    [InlineData("12345678", false)] // Too short
    [InlineData("1234567890", false)] // 10 digits (Invalid length for UIC)
    public void Uic_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = BulgariaUicValidator.ValidateUic(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Uic_Format_ReturnsNormalizedString()
    {
        Assert.Equal("121020523", BulgariaUicValidator.Format("BG 121020523"));
    }
}
