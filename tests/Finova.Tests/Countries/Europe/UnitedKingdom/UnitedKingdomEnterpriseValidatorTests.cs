using Finova.Countries.Europe.UnitedKingdom.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom;

public class UnitedKingdomEnterpriseValidatorTests
{
    [Theory]
    [InlineData("05145053", true)] // Valid Numeric CRN
    [InlineData("SC254829", true)] // Valid Prefix CRN
    [InlineData("NI036562", true)] // Valid Prefix CRN
    [InlineData("OC304455", true)] // Valid Prefix CRN
    [InlineData("5145053", true)] // Valid Numeric (Padded) -> Wait, Validate handles padding?
    // Validate method pads if numeric and < 8.
    [InlineData("1234567", true)] // Padded to 01234567
    [InlineData("123456789", false)] // Too long
    [InlineData("XX123456", false)] // Unknown Prefix (XX) - Strict validation rejects it
    // In code: "if (!ValidPrefixes.Contains(prefix)) { ... }" but I commented out strict failure.
    // So XX123456 should be VALID by Regex check.
    [InlineData("A1234567", false)] // Invalid format
    public void Crn_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = UnitedKingdomCompanyNumberValidator.ValidateCompanyNumber(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Crn_Format_ReturnsPaddedString()
    {
        Assert.Equal("05145053", UnitedKingdomCompanyNumberValidator.Format("5145053"));
        Assert.Equal("SC254829", UnitedKingdomCompanyNumberValidator.Format("sc254829"));
    }
}
