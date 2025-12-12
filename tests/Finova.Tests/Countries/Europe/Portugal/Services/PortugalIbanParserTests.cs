using Finova.Countries.Europe.Portugal.Services;
using Finova.Countries.Europe.Portugal.Validators;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Countries.Europe.Portugal.Services;

public class PortugalIbanParserTests
{
    private readonly PortugalIbanParser _parser;

    public PortugalIbanParserTests()
    {
        _parser = new PortugalIbanParser(new PortugalIbanValidator());
    }

    #region ParseIban Tests - Invalid Input

    [Theory]
    [InlineData("DE89370400440532013000")] // Wrong country
    [InlineData("PT5000020123123456789015")] // Too short
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
        var parser = PortugalIbanParser.Create();

        // Assert
        parser.Should().NotBeNull();
        parser.CountryCode.Should().Be("PT");
    }

    #endregion

    #region Consistency Tests

    [Theory]
    [InlineData("BE68539007547034")] // Belgian IBAN
    [InlineData("FR7630006000011234567890189")] // French IBAN
    [InlineData("GR1601101250000000012300695")] // Greek IBAN
    public void ParseIban_WithOtherCountryIbans_ReturnsNull(string iban)
    {
        // Act
        var result = _parser.ParseIban(iban);

        // Assert
        result.Should().BeNull();
    }

    #endregion
}

