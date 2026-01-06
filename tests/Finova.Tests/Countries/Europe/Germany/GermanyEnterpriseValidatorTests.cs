using Finova.Countries.Europe.Germany.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.Germany;

public class GermanyEnterpriseValidatorTests
{
    [Theory]
    [InlineData("1234567890123", true)] // 13 digits
    [InlineData("123456789012", false)] // 12 digits
    [InlineData("12345678901234", false)] // 14 digits
    [InlineData("123456789012A", false)] // Non-numeric
    [InlineData("", false)] // Empty
    [InlineData(null, false)] // Null
    public void Steuernummer_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GermanySteuernummerValidator.ValidateSteuernummer(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Theory]
    [InlineData("HRB 12345", true)]
    [InlineData("HRA 12345", true)]
    [InlineData("HRB12345", true)] // Normalized check
    [InlineData("HRA12345", true)]
    [InlineData("hrb 12345", true)] // Case insensitive
    [InlineData("HRB 123", true)]
    [InlineData("HRX 12345", false)] // Invalid prefix
    [InlineData("HRB", false)] // Missing digits
    [InlineData("12345", false)] // Missing prefix
    public void Handelsregisternummer_Validate_ReturnsExpectedResult(string? input, bool expectedIsValid)
    {
        var result = GermanyHandelsregisternummerValidator.ValidateHandelsregisternummer(input);
        Assert.Equal(expectedIsValid, result.IsValid);
    }

    [Fact]
    public void Handelsregisternummer_Format_ReturnsFormattedString()
    {
        Assert.Equal("HRB 12345", GermanyHandelsregisternummerValidator.Format("HRB12345"));
        Assert.Equal("HRA 12345", GermanyHandelsregisternummerValidator.Format("hra 12345"));
    }
}
