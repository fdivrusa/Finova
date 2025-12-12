using Finova.Countries.Europe.Denmark.Models;
using Finova.Countries.Europe.Denmark.Services;
using Finova.Countries.Europe.Denmark.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Denmark.Services;

public class DenmarkIbanParserTests
{
    private readonly DenmarkIbanParser _parser;

    public DenmarkIbanParserTests()
    {
        _parser = new DenmarkIbanParser(new DenmarkIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidDanishIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "DK5000400440116243";

        // Act
        var result = _parser.ParseIban(iban) as DenmarkIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("DK5000400440116243");
        result.CountryCode.Should().Be("DK");
        result.CheckDigits.Should().Be("50");
        result.Registreringsnummer.Should().Be("0040");
        result.Kontonummer.Should().Be("0440116243");
        result.BankCode.Should().Be("0040");
        result.AccountNumber.Should().Be("0440116243");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("DK50 0040 0440 1162 43")] // With spaces
    [InlineData("dk5000400440116243")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as DenmarkIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("DK5000400440116243");
    }

    [Theory]
    [InlineData("DK0000400440116243")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("DK500040044011624")] // Too short
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
        var parser = DenmarkIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("DK");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "DK5000400440116243";

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

