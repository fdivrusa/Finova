using Finova.Countries.Europe.Poland.Models;
using Finova.Countries.Europe.Poland.Services;
using Finova.Countries.Europe.Poland.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Poland.Services;

public class PolandIbanParserTests
{
    private readonly PolandIbanParser _parser;

    public PolandIbanParserTests()
    {
        _parser = new PolandIbanParser(new PolandIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidPolishIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "PL61109010140000071219812874";

        // Act
        var result = _parser.ParseIban(iban) as PolandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("PL61109010140000071219812874");
        result.CountryCode.Should().Be("PL");
        result.CheckDigits.Should().Be("61");
        result.NumerRozliczeniowyBanku.Should().Be("10901014");
        result.NumerRachunku.Should().Be("0000071219812874");
        result.BankCode.Should().Be("10901014");
        result.AccountNumber.Should().Be("0000071219812874");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("PL61 1090 1014 0000 0712 1981 2874")] // With spaces
    [InlineData("pl61109010140000071219812874")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as PolandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("PL61109010140000071219812874");
    }

    [Theory]
    [InlineData("PL00109010140000071219812874")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("PL6110901014000007121981287")] // Too short
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
    public void ParseIban_WithMBankExample_ParsesCorrectly()
    {
        // Arrange - mBank example
        var iban = "PL27114020040000300201355387";

        // Act
        var result = _parser.ParseIban(iban) as PolandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.NumerRozliczeniowyBanku.Should().Be("11402004");
        result.NumerRachunku.Should().Be("0000300201355387");
        result.CheckDigits.Should().Be("27");
    }

    [Fact]
    public void ParseIban_WithINGBankExample_ParsesCorrectly()
    {
        // Arrange - ING Bank example
        var iban = "PL60102010260000042270201111";

        // Act
        var result = _parser.ParseIban(iban) as PolandIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.NumerRozliczeniowyBanku.Should().Be("10201026");
        result.NumerRachunku.Should().Be("0000042270201111");
        result.BankCode.Should().Be("10201026");
        result.AccountNumber.Should().Be("0000042270201111");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = PolandIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("PL");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "PL61109010140000071219812874";

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
