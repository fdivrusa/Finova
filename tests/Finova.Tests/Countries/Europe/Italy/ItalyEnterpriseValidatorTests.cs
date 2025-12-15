using Finova.Countries.Europe.Italy.Validators;
using Finova.Core.Common;
using Xunit;

namespace Finova.Tests.Countries.Europe.Italy;

public class ItalyEnterpriseValidatorTests
{
    [Theory]
    [InlineData("RSSMRA85T10A562S", true)] // Valid CF (Mario Rossi)
    [InlineData("RSSMRA85T10A562X", false)] // Invalid Check Digit
    [InlineData("RSSMRA85T10A562", false)] // Too short
    [InlineData("RSSMRA85T10A562SS", false)] // Too long
    public void CodiceFiscale_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = ItalyCodiceFiscaleValidator.ValidateItalianIdentifier(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("00743110157", true)] // Valid P.IVA (Eni)
    [InlineData("00743110158", false)] // Invalid Check Digit
    [InlineData("0074311015", false)] // Too short
    [InlineData("007431101577", false)] // Too long
    [InlineData("ABCDEFGHIJK", false)] // Non-numeric
    public void PartitaIva_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = ItalyCodiceFiscaleValidator.ValidateItalianIdentifier(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }
}
