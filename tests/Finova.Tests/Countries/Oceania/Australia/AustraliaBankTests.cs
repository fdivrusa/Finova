using Finova.Countries.Oceania.Australia.Validators;
using Xunit;

namespace Finova.Tests.Countries.Oceania.Australia;

public class AustraliaBankTests
{
    [Theory]
    [InlineData("032-001", true)] // Valid BSB
    [InlineData("032001", true)] // Valid (sanitized)
    [InlineData("032 001", true)] // Valid (sanitized)
    [InlineData("032-00", false)] // Too short
    [InlineData("032-0011", false)] // Too long
    [InlineData("ABC-DEF", false)] // Non-numeric
    [InlineData(null, false)] // Null
    [InlineData("", false)] // Empty
    public void ValidateBsb_ShouldReturnExpectedResult(string? input, bool expected)
    {
        var validator = new AustraliaBsbValidator();
        var result = validator.Validate(input);
        Assert.Equal(expected, result.IsValid);
    }

    [Theory]
    [InlineData("123456", true)] // Valid (6 digits)
    [InlineData("123456789", true)] // Valid (9 digits)
    [InlineData("1234567890", true)] // Valid (10 digits - max)
    [InlineData("12345", false)] // Too short
    [InlineData("12345678901", false)] // Too long
    [InlineData("ABCDEF", false)] // Non-numeric
    [InlineData(null, false)] // Null
    [InlineData("", false)] // Empty
    public void ValidateBankAccount_ShouldReturnExpectedResult(string? input, bool expected)
    {
        var validator = new AustraliaBankAccountValidator();
        var result = validator.Validate(input);
        Assert.Equal(expected, result.IsValid);
    }
}
