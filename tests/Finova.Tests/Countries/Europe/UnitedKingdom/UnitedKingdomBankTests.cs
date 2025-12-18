using Finova.Countries.Europe.UnitedKingdom.Validators;
using Xunit;

namespace Finova.Tests.Countries.Europe.UnitedKingdom;

public class UnitedKingdomBankTests
{
    [Theory]
    [InlineData("12-34-56", true)] // Valid
    [InlineData("123456", true)] // Valid (sanitized)
    [InlineData("12 34 56", true)] // Valid (sanitized)
    [InlineData("12-34-5", false)] // Too short
    [InlineData("12-34-567", false)] // Too long
    [InlineData("AB-CD-EF", false)] // Non-numeric
    [InlineData(null, false)] // Null
    [InlineData("", false)] // Empty
    public void ValidateSortCode_ShouldReturnExpectedResult(string? input, bool expected)
    {
        var validator = new UnitedKingdomSortCodeValidator();
        var result = validator.Validate(input);
        Assert.Equal(expected, result.IsValid);
    }

    [Theory]
    [InlineData("12345678", true)] // Valid
    [InlineData("12 34 56 78", true)] // Valid (sanitized)
    [InlineData("1234567", false)] // Too short
    [InlineData("123456789", false)] // Too long
    [InlineData("ABCDEFGH", false)] // Non-numeric
    [InlineData(null, false)] // Null
    [InlineData("", false)] // Empty
    public void ValidateBankAccount_ShouldReturnExpectedResult(string? input, bool expected)
    {
        var validator = new UnitedKingdomBankAccountValidator();
        var result = validator.Validate(input);
        Assert.Equal(expected, result.IsValid);
    }
}
