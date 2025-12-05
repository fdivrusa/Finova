using Finova.Countries.Europe.Norway.Models;
using Finova.Countries.Europe.Norway.Services;
using Finova.Countries.Europe.Norway.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Norway.Services;

public class NorwayIbanParserTests
{
    private readonly NorwayIbanParser _parser;

    public NorwayIbanParserTests()
    {
        _parser = new NorwayIbanParser(new NorwayIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidNorwegianIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "NO9386011117947";

        // Act
        var result = _parser.ParseIban(iban) as NorwayIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("NO9386011117947");
        result.CountryCode.Should().Be("NO");
        result.CheckDigits.Should().Be("93");
        result.Bankkod.Should().Be("8601");
        result.Kontonummer.Should().Be("111794");
        result.Kontrollsiffer.Should().Be("7");
        result.BankCode.Should().Be("8601");
        result.AccountNumber.Should().Be("111794");
        result.NationalCheckKey.Should().Be("7");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("NO93 8601 1117 947")] // With spaces
    [InlineData("no9386011117947")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as NorwayIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("NO9386011117947");
    }

    [Theory]
    [InlineData("NO0086011117947")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("NO938601111794")] // Too short
    [InlineData("")] // Empty
    [InlineData(null)] // Null
    public void ParseIban_WithInvalidIban_ReturnsNull(string? iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = NorwayIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("NO");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "NO9386011117947";

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
