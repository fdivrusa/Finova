using Finova.Countries.Europe.Sweden.Models;
using Finova.Countries.Europe.Sweden.Services;
using Finova.Countries.Europe.Sweden.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Sweden.Services;

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
        var iban = "SE4550000000058398257466";

        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("SE4550000000058398257466");
        result.CountryCode.Should().Be("SE");
        result.CheckDigits.Should().Be("45");
        result.Bankkod.Should().Be("500");
        result.Kontonummer.Should().Be("00000058398257466");
        result.BankCode.Should().Be("500");
        result.AccountNumber.Should().Be("00000058398257466");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("SE45 5000 0000 0583 9825 7466")] // With spaces
    [InlineData("se4550000000058398257466")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("SE4550000000058398257466");
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
        var iban = "SE3550000000054910000003";

        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Bankkod.Should().Be("500");
        result.Kontonummer.Should().Be("00000054910000003");
        result.CheckDigits.Should().Be("35");
    }

    [Fact]
    public void ParseIban_WithSwedbankExample_ParsesCorrectly()
    {
        // Arrange - Swedbank example
        var iban = "SE6412000000012170145230";

        // Act
        var result = _parser.ParseIban(iban) as SwedenIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Bankkod.Should().Be("120");
        result.Kontonummer.Should().Be("00000012170145230");
        result.BankCode.Should().Be("120");
        result.AccountNumber.Should().Be("00000012170145230");
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
        var iban = "SE4550000000058398257466";

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
