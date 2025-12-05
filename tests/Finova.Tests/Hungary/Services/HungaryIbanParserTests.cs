using Finova.Countries.Europe.Hungary.Models;
using Finova.Countries.Europe.Hungary.Services;
using Finova.Countries.Europe.Hungary.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Hungary.Services;

public class HungaryIbanParserTests
{
    private readonly HungaryIbanParser _parser;

    public HungaryIbanParserTests()
    {
        _parser = new HungaryIbanParser(new HungaryIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidHungarianIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "HU42117730161111101800000000";

        // Act
        var result = _parser.ParseIban(iban) as HungaryIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("HU42117730161111101800000000");
        result.CountryCode.Should().Be("HU");
        result.CheckDigits.Should().Be("42");
        result.Bankazonosito.Should().Be("117");
        result.Fiokazonosito.Should().Be("7301");
        result.Szamlaszam.Should().Be("61111101800000000");
        result.BankCode.Should().Be("117");
        result.BranchCode.Should().Be("7301");
        result.AccountNumber.Should().Be("61111101800000000");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("HU42 1177 3016 1111 1018 0000 0000")] // With spaces
    [InlineData("hu42117730161111101800000000")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as HungaryIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("HU42117730161111101800000000");
    }

    [Theory]
    [InlineData("HU00117730161111101800000000")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("HU4211773016111110180000000")] // Too short
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
        var parser = HungaryIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("HU");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "HU42117730161111101800000000";

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
