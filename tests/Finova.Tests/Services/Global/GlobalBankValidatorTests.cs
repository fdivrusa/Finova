using Finova.Services;
using Xunit;

namespace Finova.Tests.Services.Global;

public class GlobalBankValidatorTests
{
    [Theory]
    [InlineData("US", "121000248", true)] // Valid ABA
    [InlineData("US", "123456789", false)] // Invalid Checksum
    [InlineData("CA", "000112345", true)] // Valid CA Routing (EFT: 0 + Inst + Transit)
    [InlineData("CA", "000000000", true)] // Valid EFT Format
    [InlineData("CA", "12345-001", true)] // Valid MICR Format (XXXXX-YYY)
    [InlineData("CA", "12345001", true)] // Valid MICR Format without hyphen
    [InlineData("CA", "1234-001", false)] // Invalid Format (wrong length)
    [InlineData("GB", "12-34-56", true)] // Valid UK Sort Code
    [InlineData("UK", "123456", true)] // Valid UK Sort Code (Alias)
    [InlineData("AU", "032-001", true)] // Valid AU BSB
    [InlineData("XX", "12345", false)] // Unsupported Country
    [InlineData("US", null, false)] // Null
    [InlineData("US", "", false)] // Empty
    public void ValidateRoutingNumber_ShouldReturnExpectedResult(string country, string? routing, bool expected)
    {
        // Act
        var result = GlobalBankValidator.ValidateRoutingNumber(country, routing);

        // Assert
        Assert.Equal(expected, result.IsValid);
    }

    [Theory]
    [InlineData("SG", "1234567890", true)] // Valid SG (length 8-11)
    [InlineData("SG", "123", false)] // Too short
    [InlineData("JP", "1234567", true)] // Valid JP (length 7)
    [InlineData("JP", "123456", false)] // Too short
    [InlineData("GB", "12345678", true)] // Valid UK Account
    [InlineData("UK", "12345678", true)] // Valid UK Account (Alias)
    [InlineData("AU", "123456789", true)] // Valid AU Account
    [InlineData("XX", "12345", false)] // Unsupported Country
    [InlineData("SG", null, false)] // Null
    [InlineData("SG", "", false)] // Empty
    public void ValidateAccountNumber_ShouldReturnExpectedResult(string country, string? account, bool expected)
    {
        // Act
        var result = GlobalBankValidator.ValidateBankAccount(country, account);

        // Assert
        Assert.Equal(expected, result.IsValid);
    }
}
