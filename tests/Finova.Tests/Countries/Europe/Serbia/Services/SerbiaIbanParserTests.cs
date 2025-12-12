using Finova.Countries.Europe.Serbia.Models;
using Finova.Countries.Europe.Serbia.Services;
using Finova.Countries.Europe.Serbia.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Serbia.Services;

public class SerbiaIbanParserTests
{
    private readonly SerbiaIbanParser _parser;

    public SerbiaIbanParserTests()
    {
        _parser = new SerbiaIbanParser(new SerbiaIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidSerbianIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "RS35260005601001611379";

        // Act
        var result = _parser.ParseIban(iban) as SerbiaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("RS35260005601001611379");
        result.CountryCode.Should().Be("RS");
        result.CheckDigits.Should().Be("35");
        result.BrojBanke.Should().Be("260");
        result.BrojRacuna.Should().Be("0056010016113");
        result.KontrolniBroj.Should().Be("79");
        result.BankCode.Should().Be("260");
        result.AccountNumber.Should().Be("0056010016113");
        result.NationalCheckKey.Should().Be("79");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("RS35 2600 0560 1001 6113 79")] // With spaces
    [InlineData("rs35260005601001611379")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as SerbiaIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("RS35260005601001611379");
    }

    [Theory]
    [InlineData("RS00260005601001611379")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("RS3526000560100161137")] // Too short
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
        var parser = SerbiaIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("RS");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "RS35260005601001611379";

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

