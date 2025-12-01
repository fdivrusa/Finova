using Finova.Countries.Europe.Greece.Models;
using Finova.Countries.Europe.Greece.Services;
using Finova.Countries.Europe.Greece.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Greece.Services;

public class GreeceIbanParserTests
{
    private readonly GreeceIbanParser _parser;

    public GreeceIbanParserTests()
    {
        _parser = new GreeceIbanParser(new GreeceIbanValidator());
    }

    #region ParseIban Tests

    [Fact]
    public void ParseIban_WithValidGreekIban_ReturnsCorrectDetails()
    {
        // Arrange
        var iban = "GR1601101250000000012300695";

        // Act
        var result = _parser.ParseIban(iban) as GreeceIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("GR1601101250000000012300695");
        result.CountryCode.Should().Be("GR");
        result.CheckDigits.Should().Be("16");
        result.KodikosTrapezas.Should().Be("011");
        result.KodikosKatastimatos.Should().Be("0125");
        result.ArithmosLogariasmou.Should().Be("0000000012300695");
        result.BankCode.Should().Be("011");
        result.BranchCode.Should().Be("0125");
        result.AccountNumber.Should().Be("0000000012300695");
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("GR16 0110 1250 0000 0001 2300 695")] // With spaces
    [InlineData("gr1601101250000000012300695")] // Lowercase
    public void ParseIban_WithFormattedIban_ReturnsNormalizedDetails(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban) as GreeceIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.Iban.Should().Be("GR1601101250000000012300695");
    }

    [Theory]
    [InlineData("GR0001101250000000012300695")] // Wrong check digits
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("GR160110125000000012300")] // Too short
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
        // Arrange - Different bank
        var iban = "GR9608100010000001234567890";

        // Act
        var result = _parser.ParseIban(iban) as GreeceIbanDetails;

        // Assert
        result.Should().NotBeNull();
        result!.KodikosTrapezas.Should().Be("081");
        result.KodikosKatastimatos.Should().Be("0001");
        result.ArithmosLogariasmou.Should().Be("0000001234567890");
        result.CheckDigits.Should().Be("96");
    }

    #endregion

    #region Factory Method Tests

    [Fact]
    public void CreateDefault_ReturnsValidParser()
    {
        // Act
        var parser = GreeceIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("GR");
    }

    #endregion

    #region Consistency Tests

    [Fact]
    public void ParseIban_CalledMultipleTimes_ReturnsConsistentResults()
    {
        // Arrange
        var iban = "GR1601101250000000012300695";

        // Act
        var result1 = _parser.ParseIban(iban);
        var result2 = _parser.ParseIban(iban);

        // Assert
        result1.Should().BeEquivalentTo(result2);
    }

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("FI2112345600000785")] // Finnish IBAN
    public void ParseIban_WithOtherCountryIbans_ReturnsNull(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion
}
