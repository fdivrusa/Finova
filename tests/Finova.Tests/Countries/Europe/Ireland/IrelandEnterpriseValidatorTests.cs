using Finova.Countries.Europe.Ireland.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.Ireland;

public class IrelandEnterpriseValidatorTests
{
    [Theory]
    [InlineData("IE6388047V", true)] // Valid VAT (Old format)
    [InlineData("6388047V", true)] // Valid VAT (No prefix)
    [InlineData("6388047X", false)] // Invalid Checksum
    [InlineData("1234567", false)] // Too short
    public void Validate_ValidVatNumber_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = IrelandVatNumberValidator.ValidateVatNumber(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Vat_Format_ReturnsNormalizedString()
    {
        Assert.Equal("6388047V", IrelandVatNumberValidator.Format("IE 6388047V"));
    }
}
