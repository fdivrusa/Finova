using Finova.Countries.Europe.Belgium.Models;
using Finova.Countries.Europe.Belgium.Services;
using Finova.Countries.Europe.Belgium.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Belgium.Services;

public class BelgiumIbanParserTests
{
    private readonly BelgiumIbanParser _parser;

    public BelgiumIbanParserTests()
    {
        _parser = BelgiumIbanParser.Create();
    }

    #region CountryCode Tests

    [Fact]
    public void CountryCode_ReturnsBE()
    {
        // Act
        var result = _parser.CountryCode;

        // Assert
        result.Should().Be("BE");
    }

    #endregion

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidBelgianIban_ReturnsDetails()
    {
        // Arrange
        var iban = "BE68539007547034";

        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BelgiumIbanDetails>();
    }

    [Fact]
    public void ParseIban_ExtractsAllComponents()
    {
        // Arrange
        var iban = "BE68539007547034";

        // Act
        var result = _parser.ParseIban(iban) as BelgiumIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("BE68539007547034");
        result.CountryCode.Should().Be("BE");
        result.CheckDigits.Should().Be("68");
        result.BankCode.Should().Be("539");
        result.BankCodeBe.Should().Be("539");
        result.AccountNumber.Should().Be("0075470");
        result.AccountNumberBe.Should().Be("0075470");
        result.BelgianCheckKey.Should().Be("34");
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void ParseIban_FormatsCorrectly()
    {
        // Arrange
        var iban = "BE68539007547034";

        // Act
        var result = _parser.ParseIban(iban) as BelgiumIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.FormattedBelgianAccount.Should().Be("539-0075470-34");
    }

    [Theory]
    [InlineData("BE68 5390 0754 7034")] // With spaces
    [InlineData("be68539007547034")] // Lowercase
    [InlineData("Be68 5390 0754 7034")] // Mixed case
    public void ParseIban_WithFormattedInput_NormalizesAndParses(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as BelgiumIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("BE68539007547034");
        result.BankCodeBe.Should().Be("539");
    }

    [Theory]
    [InlineData("BE00539007547034")] // Wrong check digits
    [InlineData("NL91ABNA0417164300")] // Wrong country
    [InlineData("BE685390075470")] // Too short
    [InlineData("BE6853900754703X")] // Contains letter
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
    public void ParseIban_WithWhitespaceOnly_ReturnsNull()
    {
        // Arrange
        var iban = "   ";

        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion

    #region Create Factory Method Tests

    [Fact]
    public void Create_ReturnsValidParser()
    {
        // Act
        var parser = BelgiumIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("BE");
    }

    [Fact]
    public void Create_CreatesNewInstanceEachTime()
    {
        // Act
        var parser1 = BelgiumIbanParser.Create();
        var parser2 = BelgiumIbanParser.Create();

        // Assert
        parser1.Should().NotBeSameAs(parser2);
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_WithValidator_CreatesParser()
    {
        // Arrange
        var validator = new BelgiumIbanValidator();

        // Act
        var parser = new BelgiumIbanParser(validator);

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("BE");
    }

    #endregion

    #region Edge Cases

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "BE68539007547034";

        // Act
        var result1 = _parser.ParseIban(iban) as BelgiumIbanDetails;
        var result2 = _parser.ParseIban(iban) as BelgiumIbanDetails;

        // Assert
        result1.Should().NotBeNull();
        result2.Should().NotBeNull();
        result1!.Iban.Should().Be(result2!.Iban);
        result1.BankCodeBe.Should().Be(result2.BankCodeBe);
    }

    [Theory]
    [InlineData("BE71096123456769")]
    [InlineData("BE68539007547034")]
    public void ParseIban_WithDifferentValidIbans_ParsesCorrectly(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as BelgiumIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.CountryCode.Should().Be("BE");
        result.IsValid.Should().BeTrue();
    }

    #endregion
}

