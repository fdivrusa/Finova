using Finova.Countries.Europe.Lithuania.Models;
using Finova.Countries.Europe.Lithuania.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Lithuania.Services;

public class LithuaniaIbanParserTests
{
    private readonly LithuaniaIbanParser _parser;

    public LithuaniaIbanParserTests()
    {
        _parser = LithuaniaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "LT601010012345678901";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<LithuaniaIbanDetails>();

        var lithuaniaDetails = (LithuaniaIbanDetails)details!;
        lithuaniaDetails.CountryCode.Should().Be("LT");
        lithuaniaDetails.BankoKodas.Should().Be("10100");
        lithuaniaDetails.SaskaitosNumeris.Should().Be("12345678901");
    }

    [Fact]
    public void ParseIban_WithNullIban_ReturnsNull()
    {
        _parser.ParseIban(null).Should().BeNull();
    }

    [Fact]
    public void ParseIban_WithInvalidIban_ReturnsNull()
    {
        _parser.ParseIban("invalid").Should().BeNull();
    }
}
