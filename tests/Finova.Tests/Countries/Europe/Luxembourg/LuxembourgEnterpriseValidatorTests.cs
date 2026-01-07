using Finova.Countries.Europe.Luxembourg.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Luxembourg;

public class LuxembourgEnterpriseValidatorTests
{
    [Theory]
    [InlineData("LU10000356", true)]
    public void Tva_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = LuxembourgVatValidator.ValidateVat(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    // Valid RCS numbers - Commercial companies (B prefix)
    [InlineData("B123456", true)]
    [InlineData("B1", true)]
    [InlineData("B12", true)]
    [InlineData("B12345", true)]
    // Valid RCS numbers with different prefixes
    [InlineData("A123456", true)]  // Civil companies
    [InlineData("C12345", true)]   // Branches of foreign companies
    [InlineData("D1234", true)]    // Simplified joint-stock companies (SAS)
    [InlineData("E123", true)]     // European Economic Interest Groupings (GEIE)
    [InlineData("F12", true)]      // Private foundations
    [InlineData("G1", true)]       // Agricultural associations
    [InlineData("H123456", true)]  // Hospital associations
    [InlineData("J12345", true)]   // Non-profit organizations (ASBL)
    [InlineData("R1234", true)]    // Cooperatives
    [InlineData("S123", true)]     // European companies (SE)
    [InlineData("T12", true)]      // European cooperative societies (SCE)
    // Invalid RCS numbers
    [InlineData("", false)]        // Empty
    [InlineData(null, false)]      // Null
    [InlineData("123456", false)]  // Missing letter prefix
    [InlineData("B", false)]       // Missing digits
    [InlineData("B1234567", false)] // Too many digits (max 6)
    [InlineData("X123456", false)] // Invalid prefix
    [InlineData("b123456", true)] // Lowercase is normalized to uppercase and is valid
    public void Rcs_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = LuxembourgRcsValidator.ValidateRcs(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("B123456", "B123456")]
    [InlineData("b123456", "B123456")]
    [InlineData(" B 123456 ", "B123456")]
    [InlineData("RCS B123456", "B123456")]
    [InlineData("LU B123456", "B123456")]
    public void Rcs_Normalize_ReturnsExpectedResult(string input, string expected)
    {
        var result = LuxembourgRcsValidator.Normalize(input);
        Assert.Equal(expected, result);
    }
}
