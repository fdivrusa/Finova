using Finova.Countries.Europe.Spain.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.Spain;

public class SpainEnterpriseValidatorTests
{
    [Theory]
    [InlineData("A58818501", true)] // Valid CIF (Digit control)
    [InlineData("P0800000B", true)] // Valid CIF (Letter control)
    [InlineData("A58818502", false)] // Invalid Check Digit
    [InlineData("P0800000J", false)] // Invalid Check Letter
    [InlineData("A5881850", false)] // Too short
    [InlineData("Z58818501", false)] // Invalid Entity Type
    public void Cif_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = SpainCifValidator.ValidateCif(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
