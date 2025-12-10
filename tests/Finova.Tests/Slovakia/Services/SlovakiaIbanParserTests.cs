using Finova.Countries.Europe.Slovakia.Models;
using Finova.Countries.Europe.Slovakia.Services;
using FluentAssertions;
using Xunit;

namespace Finova.Tests.Slovakia.Services;

public class SlovakiaIbanParserTests
{
    private readonly SlovakiaIbanParser _parser;

    public SlovakiaIbanParserTests()
    {
        _parser = SlovakiaIbanParser.Create();
    }

    [Fact]
    public void ParseIban_WithValidIban_ReturnsIbanDetails()
    {
        var iban = "SK8975000000000012345671";
        var details = _parser.ParseIban(iban);

        details.Should().NotBeNull();
        details.Should().BeOfType<SlovakiaIbanDetails>();

        var slovakiaDetails = (SlovakiaIbanDetails)details!;
        slovakiaDetails.CountryCode.Should().Be("SK");
        slovakiaDetails.KodBanky.Should().Be("7500");
        slovakiaDetails.Predcislie.Should().Be("000000");
        slovakiaDetails.CisloUctu.Should().Be("0012345671");
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
