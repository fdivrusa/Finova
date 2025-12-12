using Finova.Countries.Europe.Austria.Models;
using Finova.Countries.Europe.Austria.Services;
using Finova.Countries.Europe.Austria.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Austria.Services;

public class AustriaIbanParserTests
{
    private readonly AustriaIbanParser _parser;

    public AustriaIbanParserTests()
    {
        _parser = new AustriaIbanParser(new AustriaIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidAustrianIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "AT611904300234573201";

        // Act
        var result = _parser.ParseIban(iban) as AustriaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("AT611904300234573201");
        result.CountryCode.Should().Be("AT");
        result.CheckDigits.Should().Be("61");
        result.Bankleitzahl.Should().Be("19043");
        result.Kontonummer.Should().Be("00234573201");
        result.BankCode.Should().Be("19043");
        result.AccountNumber.Should().Be("00234573201");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("AT61 1904 3002 3457 3201")] // With spaces
    [InlineData("at611904300234573201")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as AustriaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("AT611904300234573201");
    }

    [Theory]
    [InlineData("AT001904300234573201")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("AT6119043002345732")] // Too short
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
    public void ParseIban_WithDifferentBankleitzahl_ParsesCorrectly()
    {
        // Arrange - Using Raiffeisen Bank example
        var iban = "AT483200000012345864";

        // Act
        var result = _parser.ParseIban(iban) as AustriaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Bankleitzahl.Should().Be("32000");
        result.Kontonummer.Should().Be("00012345864");
        result.CheckDigits.Should().Be("48");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = AustriaIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("AT");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "AT611904300234573201";

        // Act
        var result1 = _parser.ParseIban(iban);
        var result2 = _parser.ParseIban(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("IT60X0542811101000000123456")] // Italian IBAN
    public void ParseIban_WithOtherCountryIbans_ReturnsNull(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion
}

