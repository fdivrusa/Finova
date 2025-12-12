using Finova.Countries.Europe.Sweden.Models;
using Finova.Countries.Europe.Sweden.Services;
using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Sweden.Services;

public class SwedenIbanParserTests
{
    private readonly SwedenIbanParser _parser;

    public SwedenIbanParserTests()
    {
        _parser = new SwedenIbanParser(new SwedenIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidSwedishIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "SE5850012345678901234560";

        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("SE5850012345678901234560");
        result.CountryCode.Should().Be("SE");
        result.CheckDigits.Should().Be("58");
        result.Bankkod.Should().Be("500");
        result.Kontonummer.Should().Be("12345678901234560");
        result.BankCode.Should().Be("500");
        result.AccountNumber.Should().Be("12345678901234560");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE58 5001 2345 6789 0123 4560")] // With spaces
    [InlineData("se5850012345678901234560")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("SE5850012345678901234560");
    }

    [Theory]
    [InlineData("SE0050000000058398257466")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("SE455000000005839825746")] // Too short
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
    public void ParseIban_WithNordeaExample_ParsesCorrectly()
    {
        // Arrange - Nordea Bank example
        var iban = "SE0630012345678901234560";

        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Bankkod.Should().Be("300");
        result.Kontonummer.Should().Be("12345678901234560");
        result.CheckDigits.Should().Be("06");
    }

    [Fact]
    public void ParseIban_WithSwedbankExample_ParsesCorrectly()
    {
        // Arrange - Swedbank example
        var iban = "SE8280012345678901234562";

        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Bankkod.Should().Be("800");
        result.Kontonummer.Should().Be("12345678901234562");
        result.BankCode.Should().Be("800");
        result.AccountNumber.Should().Be("12345678901234562");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = SwedenIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("SE");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "SE5850012345678901234560";

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

