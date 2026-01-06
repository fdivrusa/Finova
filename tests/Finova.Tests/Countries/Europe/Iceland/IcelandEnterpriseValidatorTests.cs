using Finova.Countries.Europe.Iceland.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Iceland;

public class IcelandEnterpriseValidatorTests
{
    [Theory]
    [InlineData("010130-2989", true)] // Valid Kennitala (Individual, 1930)
    [InlineData("0101302989", true)] // Valid Kennitala (No hyphen)
    [InlineData("410130-2979", true)] // Valid Kennitala (Organization, 1930)
    [InlineData("010130-2979", false)] // Invalid Checksum
    [InlineData("010130-2809", false)] // Invalid Checksum (Remainder 1 case)
    [InlineData("320130-2989", false)] // Invalid Date (Day 32)
    [InlineData("011330-2989", false)] // Invalid Date (Month 13)
    [InlineData("12345", false)] // Too short
    public void Validate_ValidKennitala_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = IcelandKennitalaValidator.ValidateKennitala(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Kennitala_Format_ReturnsFormattedString()
    {
        Assert.Equal("010130-2989", IcelandKennitalaValidator.Format("0101302989"));
    }
}
