using Finova.Countries.Europe.Finland.Models;
using Finova.Countries.Europe.Finland.Services;
using Finova.Countries.Europe.Finland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Finland.Services;

public class FinlandIbanParserTests
{
    private readonly FinlandIbanParser _parser;

    public FinlandIbanParserTests()
    {
        _parser = new FinlandIbanParser(new FinlandIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidFinnishIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "FI2112345600000785";

        // Act
        var result = _parser.ParseIban(iban) as FinlandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("FI2112345600000785");
        result.CountryCode.Should().Be("FI");
        result.CheckDigits.Should().Be("21");
        result.Rahalaitostunnus.Should().Be("123456");
        result.Tilinumero.Should().Be("00000785");
        result.BankCode.Should().Be("123456");
        result.AccountNumber.Should().Be("00000785");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("FI21 1234 5600 0007 85")] // With spaces
    [InlineData("fi2112345600000785")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as FinlandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("FI2112345600000785");
    }

    [Theory]
    [InlineData("FI0012345600000785")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("FI21123456000007")] // Too short
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ParseIban_WithDifferentBankCode_ParsesCorrectly()
    {
        // Arrange - Using another valid Finnish IBAN
        var iban = "FI1410093000123458";

        // Act
        var result = _parser.ParseIban(iban) as FinlandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Rahalaitostunnus.Should().Be("100930");
        result.Tilinumero.Should().Be("00123458");
        result.CheckDigits.Should().Be("14");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = FinlandIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("FI");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "FI2112345600000785";

        // Act
        var result1 = _parser.ParseIban(iban);
        var result2 = _parser.ParseIban(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("AT611904300234573201")] // Austrian IBAN
    public void ParseIban_WithOtherCountryIbans_ReturnsNull(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion
}

