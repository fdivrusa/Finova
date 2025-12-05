using Finova.Countries.Europe.CzechRepublic.Models;
using Finova.Countries.Europe.CzechRepublic.Services;
using Finova.Countries.Europe.CzechRepublic.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.CzechRepublic.Services;

public class CzechRepublicIbanParserTests
{
    private readonly CzechRepublicIbanParser _parser;

    public CzechRepublicIbanParserTests()
    {
        _parser = new CzechRepublicIbanParser(new CzechRepublicIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidCzechIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "CZ6508000000192000145399";

        // Act
        var result = _parser.ParseIban(iban) as CzechRepublicIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("CZ6508000000192000145399");
        result.CountryCode.Should().Be("CZ");
        result.CheckDigits.Should().Be("65");
        result.KodBanky.Should().Be("0800");
        result.Predcisli.Should().Be("000019");
        result.CisloUctu.Should().Be("2000145399");
        result.BankCode.Should().Be("0800");
        result.BranchCode.Should().Be("000019");
        result.AccountNumber.Should().Be("2000145399");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("CZ65 0800 0000 1920 0014 5399")] // With spaces
    [InlineData("cz6508000000192000145399")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as CzechRepublicIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("CZ6508000000192000145399");
    }

    [Theory]
    [InlineData("CZ0008000000192000145399")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("CZ65080000001920001453")] // Too short
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
    public void ParseIban_WithDifferentBank_ParsesCorrectly()
    {
        // Arrange - Using Raiffeisen Bank example
        var iban = "CZ9455000000001011038930";

        // Act
        var result = _parser.ParseIban(iban) as CzechRepublicIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.KodBanky.Should().Be("5500");
        result.Predcisli.Should().Be("000000");
        result.CisloUctu.Should().Be("1011038930");
        result.CheckDigits.Should().Be("94");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = CzechRepublicIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("CZ");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "CZ6508000000192000145399";

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
